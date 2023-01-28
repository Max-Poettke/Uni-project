using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetIce1 : MonoBehaviour, IPlanet, IHp
{
    [SerializeField] private float hp;
    [SerializeField] private float armor;
    [SerializeField] private float firingRate = 0.0f;
    [SerializeField] private GameObject standardProjectile;
    
    private float initialScale;
    private float initialHP;
    private int phase = 0;
    public Transform playerTransform;

    private InLevelControl controller;

    private Coroutine miscRoutine;
    private Coroutine shrinkRoutine;
    private Coroutine shootingRoutine;
    void Start()
    {
        initialHP = hp;
        initialScale = transform.localScale.x;
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
        //miscRoutine = StartCoroutine(TryGetPlayerTransform());
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
        Shrink();
    }

    public void FireProjectile()
    {
        var projectile = Instantiate(standardProjectile);
        projectile.transform.position = transform.position;
        projectile.transform.LookAt(playerTransform.position);
        float rand = Random.Range(-30, 30);
        projectile.transform.Rotate(0, 0, rand);
    }

    public void Shrink()
    {
        if (hp < (initialHP / 3) * 2 && phase == 0)
        {
            phase = 1;
            if (shrinkRoutine == null)
            {
                shrinkRoutine = StartCoroutine(ShrinkPlanet());
                return;
            }
            StopCoroutine(shrinkRoutine);
            StartCoroutine(ShrinkPlanet());
        }

        if (hp < (initialHP / 3) && phase == 1)
        {
            phase = 2;
            if (shrinkRoutine == null)
            {
                shrinkRoutine = StartCoroutine(ShrinkPlanet());
                return;
            }
            StopCoroutine(shrinkRoutine);
            StartCoroutine(ShrinkPlanet());
        }

        if (hp <= 0)
        {
            phase = 3;
            if (shrinkRoutine == null)
            {
                shrinkRoutine = StartCoroutine(ShrinkPlanet());
                return;
            }
            StopCoroutine(shrinkRoutine);
            StartCoroutine(ShrinkPlanet());
        }
    }
    
    public IEnumerator ShrinkPlanet()
    {
        float shrinkage = (hp) / initialScale;
        if (shrinkage < 0)
        {
            shrinkage = 0;
        }
        float timer = 0;
        //float shrinkage = ((transform.localScale.x - amountToShrink) < 0) ? 0 : transform.localScale.x - amountToShrink;
        var newPos = transform.position -= new Vector3(shrinkage / 30, 0f, 0f);
        
        while (transform.localScale.x > shrinkage)
        {
            timer += Time.deltaTime;
            float newScale = Mathf.Lerp(transform.localScale.x, shrinkage, timer);
            transform.position = Vector3.Lerp(transform.position, newPos, timer);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return new WaitForFixedUpdate();
        }

        if (phase == 3)
        {
            Die();
        }
    }

    private IEnumerator TryGetPlayerTransform()
    {
        while (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            yield return new WaitForFixedUpdate();
        }
        shootingRoutine = StartCoroutine(FireContinuously());
    }

    public IEnumerator FireContinuously()
    {
        while (true)
        {
            yield return new WaitForSeconds(firingRate);
            FireProjectile();
        }
        
    }
}
