using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour, IGun
{
    [SerializeField] private GameObject projectile;
    public float firingCooldown = 0.7f;
    [SerializeField] private float timer = 0;
    [SerializeField] bool cantFire = false;
    
    public float dmgIncrement = 1f;
    public float penetrationIncrement = 1f;
    public float firingIncrement = 1.2f; 
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
        for (int i = 0; i < 8; i++)
        {
            var proj = Instantiate(projectile, transform.position, transform.rotation);
            float angle = (40f / 8) * i - 20f;
            proj.transform.Rotate(Vector3.back, angle, Space.Self);
        }
        cantFire = true;
    }

    public void Upgrade(int dmgIndex, int penetrationIndex, int firingIndex)
    {
        var projScript = projectile.GetComponent<ShotGunProjectile>();
        projScript.damage += dmgIncrement * firingIndex;
        projScript.hp += (int) penetrationIncrement * penetrationIndex;
        for (int i = 0; i < firingIndex; i ++)
        {
            firingCooldown /= firingIncrement;
        }
    }
}
