using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour, ITrinket
{
    [SerializeField] private GameObject fieldObject;
    [SerializeField] private float maxCooldown;
    [SerializeField] private float duration;
    [SerializeField] private TrailRenderer trailRenderer;
    private float cooldown = 0;
    private Coroutine forceFieldRoutine;

    private void Start()
    {
        StartCoroutine(RefreshCooldown());
    }

    public void Use()
    {
        if (forceFieldRoutine == null)
        {
            forceFieldRoutine = StartCoroutine(ActivateForceField());
            return;
        }
        StopCoroutine(forceFieldRoutine);
        forceFieldRoutine = StartCoroutine(ActivateForceField());
    }

    IEnumerator ActivateForceField()
    {
        Debug.Log("started the coroutine");
        if (cooldown > 0) yield break;
        Debug.Log("should activate");
        fieldObject.SetActive(true);
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(duration);
        fieldObject.SetActive(false);
        cooldown = maxCooldown;
    }

    IEnumerator RefreshCooldown()
    {
        while (true)
        {
            if (cooldown < 0)
            {
                trailRenderer.emitting = true;
            }
            else
            {
                trailRenderer.emitting = false;
            }
            cooldown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
