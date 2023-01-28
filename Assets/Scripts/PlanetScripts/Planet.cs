using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour, IHp
{
    [SerializeField] private float hp;
    private float initialScale;
    private InLevelControl controller;
    private Coroutine shrinkingRoutine;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
        initialScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage, float armorPenetrationFactor)
    {
        damage *= armorPenetrationFactor;
        if (shrinkingRoutine == null)
        {
            shrinkingRoutine = StartCoroutine(ShrinkPlanet(damage));
            return;
        }
        StopCoroutine(shrinkingRoutine);
        shrinkingRoutine = StartCoroutine(ShrinkPlanet(damage));
    }

    public void Die()
    {
        //Ends the level
        controller.levelCompleted = true;
        Destroy(gameObject);
    }

    public IEnumerator ShrinkPlanet(float damage)
    {
        float amountToShrink = (hp - damage) / initialScale;
        hp -= damage;
        float timer = 0;
        float shrinkage = ((transform.localScale.x - amountToShrink) < 0) ? 0 : transform.localScale.x - amountToShrink;
        transform.position -= new Vector3(0.1f, 0f, 0f);
        while (true)
        {
            timer += Time.deltaTime;
            shrinkage = amountToShrink;
            float newScale = Mathf.Lerp(transform.localScale.x, shrinkage, timer);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return new WaitForFixedUpdate();
        }
    }
}
