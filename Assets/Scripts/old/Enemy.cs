using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    Vector2 minMaxSpeed;
    [SerializeField]
    Vector3 rotationSpeed;
    // Start is called before the first frame update
    private Vector3 initalScale;
    void Start()
    {
        // save initial scale to adjust it in each run
        initalScale = transform.localScale;
        SetSpeedAndPosition();
    }

    // Update is called once per frame
    void Update()
    {
        float amtToMove = Time.deltaTime * speed;
        transform.Translate(Vector3.down * amtToMove, Space.World);
        transform.Rotate(Time.deltaTime* rotationSpeed, Space.Self);

        if( Camera.main.WorldToViewportPoint( transform.position).y < -0.1f)
        {
            SetSpeedAndPosition();
        }
    }

    private float speed;
    public void SetSpeedAndPosition()
    {
        speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);

        Vector3 newPosition = Camera.main.ViewportToWorldPoint( new Vector3(Random.Range(0.05f,0.95f), 1, 0));
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        transform.localScale =  new Vector3(Random.Range(initalScale.x  * 0.8f, initalScale.x*1.3f),Random.Range(initalScale.y  * 0.8f,initalScale.y*1.3f),Random.Range(initalScale.z * 0.8f, initalScale.z*1.3f));
    }
}
