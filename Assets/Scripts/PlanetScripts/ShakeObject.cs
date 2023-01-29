using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    private Coroutine shakeRoutine;
    private Vector3 pos;
    public void ShakeNow(float duration, float magnitude)
    {
        pos = transform.localPosition;
        if (shakeRoutine == null)
        {
            shakeRoutine = StartCoroutine(Shake(duration, magnitude));
            return;
        }
        StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }
    private IEnumerator Shake(float duration, float magnitude)
    {
        
        float timer = 0;
        while (timer < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(transform.position.x + x, transform.position.y + y, pos.z);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        transform.localPosition = pos;
        StopCoroutine(shakeRoutine);
        Debug.Log("Stopped");
    }
}
