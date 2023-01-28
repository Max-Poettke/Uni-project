using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardProjectile : MonoBehaviour , IMoveable, IKillable
{
    //public GameObject explosionPrefab;
    public GameObject explosionPrefab;
    public Transform targetTransform;
    public float speed = 2f;
    public float armorPenetrationFactor = 1f;
    public float damage = 5f;
    public float lifeTime = 2f;
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
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
        transform.LookAt(targetTransform);
        transform.position += speed * Time.deltaTime * transform.up;
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
        Destroy(this.gameObject);
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IHp killableObject))
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            killableObject.TakeDamage(damage, armorPenetrationFactor);
            Die();
        }
    }
}
