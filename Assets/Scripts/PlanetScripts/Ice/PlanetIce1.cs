using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetIce1 : MonoBehaviour, IPlanet, IHp
{
    [SerializeField] private float hp;
    [SerializeField] private float armor;
    [SerializeField] private float firingRate = 0.0f;
    [SerializeField] private GameObject standardProjectile;

    public Slider slider;
    private float initialScale;
    private float initialHp;
    private int phase = 0;
    public Transform playerTransform;
    
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
        }
    }
    
    public void Die()
    {
        controller.levelCompleted = true;
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
        var projectile = Instantiate(standardProjectile);
        projectile.transform.position = transform.position;
        projectile.transform.LookAt(playerTransform.position);
        float rand = Random.Range(-40, 40);
        projectile.transform.Rotate(rand, 0, 0);
    }

    public void OneThird()
    {
        armor++;
        firingRate -= 0.05f;
        Debug.Log("OneThird called");
    }

    public void TwoThirds()
    {
        armor += 2;
        firingRate -= 0.05f;
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
