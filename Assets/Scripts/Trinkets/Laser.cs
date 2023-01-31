using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, ITrinket
{
    [SerializeField] private float damage;
    public float unlockPrice = 500;
    private RaycastHit[] hit;
    
    private InLevelControl controller;
    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
    }

    public void Use()
    {
        Ray ray = new Ray(transform.position, controller.instantiatedPlanet.transform.position);
        hit = Physics.RaycastAll(ray);
        for(int i = 0; i < hit.Length; i ++)
        {
            if (hit[i].transform.CompareTag("Projectile"))
            {
                hit[i].transform.TryGetComponent(out IHp hpScript);
                hpScript.Die();
                continue;
            }

            hit[i].transform.TryGetComponent(out IHp hpScript2);
            hpScript2.TakeDamage(damage, 50);
        }
    }
}
