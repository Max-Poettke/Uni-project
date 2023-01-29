using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOverlaps : MonoBehaviour
{
    public IPlanet planetScript;
    public GameObject planet;
    private Coroutine shrinkRoutine;
    public void Shrink(float hp, float initialHp, float initialScale)
    {
        if (hp < (initialHp / 3) * 2 && planetScript.GetPhase() == 0)
        {
            planetScript.SetPhase(1);
            planetScript.OneThird();
            if (shrinkRoutine == null)
            {
                shrinkRoutine = StartCoroutine(ShrinkPlanet(hp, initialHp, initialScale, planetScript.GetPhase()));
                return;
            }
            StopCoroutine(shrinkRoutine);
            StartCoroutine(ShrinkPlanet(hp, initialHp, initialScale, planetScript.GetPhase()));
        }

        if (hp < (initialHp / 3) && planetScript.GetPhase() == 1)
        {
            planetScript.SetPhase(2);
            planetScript.TwoThirds();
            if (shrinkRoutine == null)
            {
                shrinkRoutine = StartCoroutine(ShrinkPlanet(hp, initialHp, initialScale, planetScript.GetPhase()));
                return;
            }
            StopCoroutine(shrinkRoutine);
            shrinkRoutine = StartCoroutine(ShrinkPlanet(hp, initialHp, initialScale, planetScript.GetPhase()));
        }

        if (hp <= 0)
        {
            planetScript.SetPhase(3);
            if (shrinkRoutine == null)
            {
                shrinkRoutine = StartCoroutine(ShrinkPlanet(hp, initialHp, initialScale, planetScript.GetPhase()));
                return;
            }
            StopCoroutine(shrinkRoutine);
            StartCoroutine(ShrinkPlanet(hp, initialHp, initialScale, planetScript.GetPhase()));
        }
    }

    public Transform TryGetPlayerTransform()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private IEnumerator ShrinkPlanet(float hp, float initialHp, float initialScale, int phase)
    {
        float shrinkage = (hp / initialHp) * initialScale;
        if (shrinkage < 0)
        {
            shrinkage = 0;
        }
        float timer = 0;
        var newPos = planet.transform.position -= new Vector3(shrinkage / 25, 0f, 0f);
        Debug.Log(planet.transform.localScale.x + " : " + shrinkage);
        while (timer < 1.1f)
        {
            timer += Time.deltaTime;
            float newScale = Mathf.Lerp(planet.transform.localScale.x, shrinkage, timer);
            planet.transform.position = Vector3.Lerp(planet.transform.position, newPos, timer);
            planet.transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return new WaitForFixedUpdate();
        }

        if (phase == 3)
        {
            planetScript.Die();
        }
    }
}
