using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardIceProjectile : MonoBehaviour
{
    //public GameObject explosionPrefab;
    public float speed = 2f;
    public float lifeTime = 3f;
    private float timer = 0f;
    private InLevelControl controller;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
    }
    void Update()
    {
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
    public void Die()
    {
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("collision detected");
        if(col.TryGetComponent(out IShip player))
        {
            player.Die();
            Die();
        }
    }
}
