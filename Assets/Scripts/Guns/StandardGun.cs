using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class StandardGun : MonoBehaviour, IGun
{
    [SerializeField] private GameObject projectile;
    public float firingCooldown = 0.3f;
    private float timer = 0;
    private bool cantFire = false;
    
    public float dmgIncrement = 5f;
    public float penetrationIncrement = 1f;
    public float firingIncrement = 1.3f; 
    public int dmgUpgrade { get; set; }
    public int firingUpgrade {get; set; }
    public int penetrationUpgrade {get; set; }

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

    public void Upgrade()
    {
        var projScript = projectile.GetComponent<StandardProjectile>();
        projScript.damage += dmgIncrement;
        projScript.hp += (int) penetrationIncrement;
        firingCooldown /= firingIncrement;
    }
}
