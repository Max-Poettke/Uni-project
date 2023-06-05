using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunProjectile : MonoBehaviour, IMoveable, IKillable
{
    //public GameObject explosionPrefab;
    public int hp = 5;
    public GameObject explosionPrefab;
    public Transform targetTransform;
    public float speed = 2f;
    public float armorPenetrationFactor = 1f;
    public float damage = 3f;
    public float lifeTime = 0.3f;
    private float lifeTimer = 0f;
    private InLevelControl controller;

    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isPaused) return;
        if (controller.died) return;
        Move();
        Expire();
    }

    void Expire()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
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
        Destroy(gameObject);
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out ShakeObject shakeScript);
        if (shakeScript != null && !other.CompareTag("Player"))
        {
            shakeScript.ShakeNow(0.3f,0.05f);
        }
        
        if (other.CompareTag("Vulnerability"))
        {
            //collision detection for vulnerabilities
            other.GetComponent<Vulnerability>().TakeDamage(damage, armorPenetrationFactor);
            controller.hitVulnerabilities++;
            return;
        }
        
        if (other.gameObject.transform.CompareTag("Projectile"))
        {
            //collision detection for projectiles
            other.TryGetComponent(out IHp killableProjectile);
            if (killableProjectile == null) return;
            killableProjectile.TakeDamage(1,0);
            hp--;
            if (hp == 0)
            {
                Die();
            }
            controller.destroyedProjectiles++;
            
            return;
        }
        
        if(other.TryGetComponent(out IHp killableObject))
        {
            //Collision detection for planets
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            killableObject.TakeDamage(damage, armorPenetrationFactor);
            Die();
        }
    }
}
