using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxScript : MonoBehaviour
{
    private RectTransform rTransform;
    private Vector3 newScale;
    private float scale = 0f;
    private bool growing = true;
    private float timer = 0f;
    void Start()
    {
        rTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (growing)
        {
            timer += Time.deltaTime;
            scale = Mathf.Lerp(20f, 50f, timer / 10);
            newScale = new Vector3(scale,scale,scale);
            rTransform.localScale = newScale;
            if (transform.localScale.x >= 50)
            {
                growing = false;
                timer = 0;
            }
            return;
        }
        
        timer += Time.deltaTime;
        scale = Mathf.Lerp(50f, 20f, timer / 10);
        newScale = new Vector3(rTransform.localScale.x - 0.1f, rTransform.localScale.y - 0.1f, rTransform.localScale.z - 0.1f);
        rTransform.localScale = newScale;
        if (transform.localScale.x <= 20)
        {
            growing = true;
            timer = 0;
        }
    }
}
