using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardEnergyProjectile : MonoBehaviour, IHp
{
    //public GameObject explosionPrefab;
    public float hp = 5;
    public float speedMin = 1.5f;
    public float speedMax = 3f;
    public float speed = 0f;
    public float lifeTime = 3f;
    private float timer = 0f;
    private InLevelControl controller;

    public Transform lightningStart;
    public Transform lightningEnd;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
        speed = Random.Range(speedMin, speedMax);
        var lightningSize = Random.Range(0.5f, 2f);
        lightningStart.localPosition = new Vector3(0, lightningSize, 0);
        lightningEnd.localPosition = new Vector3(0, -lightningSize, 0);

        var collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(0.1f, 2 * lightningSize, 0.1f);
    }
    void Update()
    {
        if (controller.isPaused) return;
        if (controller.died) return;
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
        if(col.TryGetComponent(out IShip player))
        {
            player.Die();
            Die();
        }
    }
}
