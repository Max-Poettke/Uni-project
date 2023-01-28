using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject target;
    private Camera _camera;
    private BoxCollider2D col;
    private float colSizeX = 0;
    private float colSizeY = 0;
    private float colRatio = 0;
    private Transform targetTransform;
    private StandardShip targetScript;
    private float timePassed;
    void Start()
    {
        _camera = GetComponent<Camera>();
        col = GetComponent<BoxCollider2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        targetTransform = target.transform;
        targetScript = target.GetComponent<StandardShip>();
    }

    // Update is called once per frame
    void Update()
    {
        colSizeY = _camera.orthographicSize * 2;
        colRatio = (float) Screen.width / (float) Screen.height;
        colSizeX = colRatio * colSizeY;
        col.size = new Vector2(colSizeX, colSizeY);

        if (targetScript.isMoving)
        {
            //timePassed = 0;
        }
        timePassed += Time.deltaTime;
        float x = Mathf.Lerp(transform.position.x, targetTransform.position.x, timePassed * 2);
        float y = Mathf.Lerp(transform.position.y, targetTransform.position.y, timePassed * 2);
        transform.position = new Vector3(x, y, transform.position.z);
    }
}