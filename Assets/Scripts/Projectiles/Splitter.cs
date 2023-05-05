using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Splitter : MonoBehaviour, IHp, ISplitter, ISpeedChangeable
{
    static float chanceToSplit = 1;
    private float hp = 2;
    [SerializeField] private int releasedProjectiles = 8;
    private float speed = 1.3f;
    public float lifeTime = 2f;
    private float timer = 0f;
    private InLevelControl controller;
    [SerializeField] private GameObject standardProjectile;

    public void SetChanceToSplit(float chance)
    {
        chanceToSplit = chance;
    }
    
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
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if (Random.Range(0, 1) < chanceToSplit)
        {
            for (int i = 0; i < releasedProjectiles; i++)
            {
                var inst = Instantiate(standardProjectile);
                if (inst.TryGetComponent(out ISpeedChangeable speedChangeable))
                {
                    speedChangeable.SetSpeed(speed);
                }
                inst.transform.position = transform.position;
                inst.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform);
                inst.transform.Rotate(Random.Range(-180f,180f), 0, 0);
            }
        }
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
