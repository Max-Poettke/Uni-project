using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvulnerableProjectile : MonoBehaviour, ISpeedChangeable
{
    //public GameObject explosionPrefab;
    public float speedMin = 1.5f;
    public float speedMax = 3f;
    private float speed = 0f;
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
        Vector3 posInViewSpace = Camera.main.WorldToViewportPoint(transform.position);
        if(posInViewSpace.y > 1.2f || posInViewSpace.x > 1.2f)
        {
            Die();
        }
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

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetSpeed(float minSpeed, float maxSpeed)
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }
    
    public float GetSpeed(){return speed;}
}
