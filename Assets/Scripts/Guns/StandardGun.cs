using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardGun : MonoBehaviour, IGun
{
    [SerializeField] private GameObject projectile;
    public float firingCooldown = 0.3f;
    public float damage;
    public int penetrationFactor;
    private float timer = 0;
    private bool cantFire = false;

    private void Update()
    {
        if (cantFire)
        {
            timer += Time.deltaTime;
        }
        
        if (timer >= firingCooldown)
        {
            cantFire = false;
            timer = 0;
        }
    }

    public void Fire()
    {
        if (cantFire) return;
        Instantiate(projectile, transform.position, transform.rotation);
        cantFire = true;
    }

    private void instantiateProjectile(GameObject projectile)
    {
        
    }
}
