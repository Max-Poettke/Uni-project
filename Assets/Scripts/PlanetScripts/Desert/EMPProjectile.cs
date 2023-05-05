using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPProjectile : MonoBehaviour, IHp
{
    public AudioSource stunAudio;
    public float speed = 1f;
    public float lifeTime = 3f;
    private float timer = 0f;
    private InLevelControl controller;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
    }
    void Update()
    {
        if (controller.isPaused) return;
        if (controller.died) return;
        if (controller.levelCompleted) return;
        Move();
        Expire();
    }

    void Expire()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Die();
        }
    }

    public void Move()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }
    void FixedUpdate()
    {
        // ReceiveGI screen = Camera.main.pixelRect;
        // check if projectile is inside camera view, view space is always 0 -> 1
        Vector3 posInViewSpace = Camera.main.WorldToViewportPoint(transform.position);
        if(posInViewSpace.y > 1.2f || posInViewSpace.x > 1.2f)
        {
            Die();
        }
    }

    public void TakeDamage(float damage, float armorPenetrationFactor)
    {
        // Indistructible
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            controller.stunCoroutine = StartCoroutine(controller.Stun(1.0f));
            stunAudio.Play();
        }
        if(col.TryGetComponent(out ShakeObject playerShaker))
        {
            playerShaker.ShakeNow(1.0f, 0.1f);
        }

    }
}
