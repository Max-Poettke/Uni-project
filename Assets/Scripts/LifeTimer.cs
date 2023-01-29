using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    public float timeToLive;
    void Start()
    {
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
