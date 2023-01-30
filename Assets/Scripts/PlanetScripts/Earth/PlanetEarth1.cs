using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetEarth1 : MonoBehaviour, IPlanet
{
    [SerializeField] private float hp;
    [SerializeField] private float armor;
    [SerializeField] private float firingRate = 0.0f;
    [SerializeField] private GameObject standardProjectile;
    [SerializeField] private GameObject boulderProjectile;
    [SerializeField] private GameObject vulnerabilityPrefab;
    [SerializeField] private Transform distanceKeeperTransform;
    [SerializeField] private float defaultProjectileSpeed;

    private Slider slider;
    private float chanceToSpawnBoulder = 0;
    private float initialScale;
    private float initialHp;
    private int phase = 0;
    private bool isWave = false;
    public Transform playerTransform;
    private GameObject vulnerability;

    private Coroutine vulnerabilityRoutine;
    private Coroutine shootingRoutine;
    private InLevelControl controller;
    private PlanetOverlaps planetOverlaps;
    void Start()
    {
        standardProjectile.GetComponent<IHp>();
        planetOverlaps = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PlanetOverlaps>();
        planetOverlaps.planet = gameObject;
        planetOverlaps.planetScript = this;
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
        if (controller.gameState != SceneManagement.GameStates.InLevel) return;
        initialHp = hp;
        initialScale = transform.localScale.x;
        playerTransform = planetOverlaps.TryGetPlayerTransform();
        if (playerTransform != null)
        {
            shootingRoutine = StartCoroutine(FireContinuously());
            vulnerabilityRoutine = StartCoroutine(InstantiateVulnerability());
        }
    }
    
    public void Die()
    {
        controller.levelCompleted = true;
        if (vulnerability != null)
        {
            Destroy(vulnerability);
        }
        Destroy(gameObject);
    }

    public void TakeDamage(float damage, float armorPenetrationFactor)
    {
        if (armor != 0)
        {
            damage *= armorPenetrationFactor / armor;
        }
        hp -= damage;
        slider.value = hp;
        planetOverlaps.Shrink(hp, initialHp, initialScale);
    }

    public void FireProjectile()
    {
        if (controller.died) return;
        GameObject projectile;
        if (Random.Range(0f, 1f) < chanceToSpawnBoulder)
        {
            projectile = Instantiate(boulderProjectile);
        }
        else
        {
            projectile = Instantiate(standardProjectile);    
        }

        projectile.TryGetComponent(out ISpeedChangeable speedChangeable);
        if (speedChangeable != null)
        {
            if (isWave)
            {
                speedChangeable.SetSpeed(defaultProjectileSpeed);
            }
            else
            {
                speedChangeable.SetSpeed(defaultProjectileSpeed - 0.5f, defaultProjectileSpeed + 0.5f);
            }
        }
        projectile.transform.position = transform.position;
        projectile.transform.LookAt(playerTransform.position);
        float rand = Random.Range(-40, 40);
        projectile.transform.Rotate(rand, 0, 0);
    }

    public void OneThird()
    {
        armor++;
        firingRate -= 0.07f;
        chanceToSpawnBoulder = 1f / 6f;
        Destroy(vulnerability);
        SpawnWave(8);
        Debug.Log("OneThird called");
    }

    public void TwoThirds()
    {
        armor += 2;
        firingRate -= 0.07f;
        chanceToSpawnBoulder = 2f / 6f;
        Destroy(vulnerability);
        SpawnWave(15);
        Debug.Log("TwoThirds called");
    }
    
    public IEnumerator FireContinuously()
    {
        while (!controller.died)
        {
            yield return new WaitForSeconds(firingRate);
            FireProjectile();
        }
    }

    public void SpawnWave(int amount)
    {
        isWave = true;
        while (amount > 0)
        {
            FireProjectile();
            amount--;
        }
        isWave = false;
    }

    public IEnumerator InstantiateVulnerability()
    {
        while (!controller.died)
        {
            yield return new WaitForSeconds(Random.Range(3, 6));
            if (vulnerability == null)
            {
                vulnerability = Instantiate(vulnerabilityPrefab);
                var vScript = vulnerability.GetComponent<Vulnerability>();
                vScript.planet = this;
                vulnerability.transform.parent = gameObject.transform;
                vulnerability.transform.position = transform.position;
                float distance = Vector3.Distance(vulnerability.transform.position, distanceKeeperTransform.position);
                vulnerability.transform.LookAt(playerTransform);
                vulnerability.transform.Rotate(Random.Range(-35f, 5f), 0f, 0f);
                vulnerability.transform.position = transform.position + vulnerability.transform.forward * distance;
            }
        }
    }

    public void SetSlider(Slider nSlider)
    {
        slider = nSlider;
        slider.minValue = 0f;
        slider.maxValue = hp;
        slider.value = hp;
    }
    public void SetPhase(int i) { phase = i; }
    public int GetPhase(){return phase;}
}
