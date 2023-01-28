using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSelection : MonoBehaviour
{
    public int worldIndex = 0;
    [SerializeField] private Transform[] planetTransforms = new Transform[4];
    [SerializeField] private GameObject planetCenter;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] public RawImage blackScreen;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject[] planets_Ice = new GameObject[3];
    [SerializeField] private GameObject[] planets_Earth = new GameObject[3];
    [SerializeField] private GameObject[] planets_Desert = new GameObject[3];
    [SerializeField] private GameObject[] planets_Alien = new GameObject[3];
    private GameObject[][] planetsWhole = new GameObject[4][];
    private Coroutine selectionCoroutine;
    private SceneManagement sceneManagementScript;
    private LevelSelection levelSelectionScript;
    float targetRotationY = 0;
    void Start()
    {
        sceneManagementScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
        levelSelectionScript = GetComponent<LevelSelection>();
        planetsWhole[0] = planets_Ice;
        planetsWhole[1] = planets_Earth;
        planetsWhole[2] = planets_Desert;
        planetsWhole[3] = planets_Alien;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInputs();
    }

    void CheckForInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Leaving");
            StartCoroutine(sceneManagementScript.LeaveScene("MainMenu", blackScreen));
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            worldIndex = (worldIndex + 3) % 4;
            if (selectionCoroutine != null)
            {
                StopCoroutine(selectionCoroutine);
                selectionCoroutine = StartCoroutine(rotateOnePlanet(true));
                return;
            }
            selectionCoroutine = StartCoroutine(rotateOnePlanet(true));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            worldIndex = (worldIndex + 1) % 4;
            if (selectionCoroutine != null)
            {
                StopCoroutine(selectionCoroutine);
                selectionCoroutine = StartCoroutine(rotateOnePlanet(false));
                return;
            }
            selectionCoroutine = StartCoroutine(rotateOnePlanet(false));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectionCoroutine != null)
            {
                StopCoroutine(selectionCoroutine);
                StartCoroutine(selectWorld());
            }
            StartCoroutine(selectWorld());
        }
    }

    private IEnumerator rotateOnePlanet(bool left)
    {
        targetRotationY = (left) ? targetRotationY - 90: targetRotationY + 90;
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            planetCenter.transform.rotation = Quaternion.Slerp(planetCenter.transform.rotation,
                Quaternion.Euler(new Vector3(0f, targetRotationY, 0f)), timer / 4);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator selectWorld()
    {
        text.SetActive(false);
        float alpha = 0f;
        float timer = 0f;
        while (alpha < 1f)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(alpha, 1f, timer / 3f);
            //Debug.Log(alpha);
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, planetTransforms[worldIndex].position, timer / 50f);
            Color currColor = blackScreen.color;
            currColor.a = alpha;
            blackScreen.color = currColor;
            yield return new WaitForFixedUpdate();
        }
        enabled = false;
        sceneManagementScript.LoadScene("LevelSelection");
        //Debug.Log(worldIndex);
        //Debug.Log(planetsWhole[worldIndex]);
        levelSelectionScript.planets = planetsWhole[worldIndex];
        levelSelectionScript.indicators = new GameObject[3];
        levelSelectionScript.enabled = true;
    }
}
