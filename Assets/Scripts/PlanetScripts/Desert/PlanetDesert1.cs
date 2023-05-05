using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetDesert1 : MonoBehaviour, IPlanet, IHp
{
    [SerializeField] private float hp;
    [SerializeField] private float armor;
    [SerializeField] private float firingRate = 0.0f;
    [SerializeField] private GameObject standardProjectile;
    [SerializeField] private GameObject empProjectile;
    [SerializeField] private GameObject vulnerabilityPrefab;
    [SerializeField] private Transform distanceKeeperTransform;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private ParticleSystem deathAnimation;

    private bool enraged = false;
    public Slider slider;
    private float initialScale;
    private float initialHp;
    private int phase = 0;
    public Transform playerTransform;
    private GameObject vulnerability;

    private Coroutine vulnerabilityRoutine;
    private Coroutine shootingRoutine;
    private InLevelControl controller;
    private PlanetOverlaps planetOverlaps;
    void Start()
    {
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
        deathSound.Play();
        deathAnimation.Play();
        this.enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
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
        var projectile = Instantiate(standardProjectile);
        projectile.transform.position = transform.position;
        projectile.transform.LookAt(playerTransform.position);
        float rand = Random.Range(-40, 40);
        projectile.transform.Rotate(rand, 0, 0);
    }
    
    private void Stun()
    {
        if (controller.died) return;
        for (int i = -40; i <= 40; i+=5)
        {
            var projectile = Instantiate(empProjectile);
            projectile.transform.position = transform.position;
            projectile.transform.LookAt(playerTransform.position);
            projectile.transform.Rotate(i, 0, 0);
        }
    }

    public void OneThird()
    {
        Stun();
        armor++;
        firingRate -= 0.05f;
        Debug.Log("OneThird called");
    }

    public void TwoThirds()
    {
        Stun();
        armor++;
        enraged = true;
        firingRate -= 0.05f;
        StartCoroutine(StunContinuously());
        Debug.Log("TwoThirds called");
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
    
    public IEnumerator FireContinuously()
    {
        while (!controller.died)
        {
            yield return new WaitForSeconds(firingRate);
            FireProjectile();
        }
    }
    
    public IEnumerator StunContinuously()
    {
        while (!controller.died)
        {
            yield return new WaitForSeconds(7);
            if (enraged)
            {
                Stun();    
            }
        }
    }

    public void SetSlider(Slider nSlider)
    {
        slider = nSlider;
        slider.minValue = 0f;
        slider.maxValue = hp;
        slider.value = slider.maxValue;
    }
    public void SetPhase(int i) { phase = i; }
    public int GetPhase(){return phase;}
}
