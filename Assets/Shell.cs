using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour, IHp
{
    private float hp = 200;
    private Coroutine shellRoutine;

    [SerializeField] private AudioSource deathSound;
    // Start is called before the first frame update

    void Awake()
    {
        float targetScale = transform.localScale.x;
        transform.localScale = new Vector3(0, 0, 0);
        shellRoutine = StartCoroutine(ScaleShell(1.5f, targetScale, false));
    }
    public void TakeDamage(float damage, float armorPenetrationFactor)
    {
        hp -= damage * armorPenetrationFactor;
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        deathSound.Play();
        shellRoutine = StartCoroutine(ScaleShell(1.5f, 0f, true));
    }
    
    public IEnumerator ScaleShell(float time, float targetScale, bool die)
    {
        float timer = 0;
        float scale = 0;
        float target = targetScale; 
        while (timer < time)
        {
            timer += Time.deltaTime;
            scale = Mathf.Lerp(scale, target, timer);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return new WaitForFixedUpdate();
        }

        if (die)
        {
            Destroy(this);
        }
        StopCoroutine(shellRoutine);
    }
}
