using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public RawImage blackScreen;
    private SceneManagement sceneManagementScript;
    private int selected = 0;
    public GameObject [] planets;
    public GameObject[] indicators;
    public GameObject cameraObject;
    private Coroutine selectionCoroutine;
    private InLevelControl controlScript;
    
    

    void Start()
    {
        controlScript = gameObject.GetComponent<InLevelControl>();
        sceneManagementScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
    }

    void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Leaving");
            //StartCoroutine(sceneManagementScript.LeaveScene("MainMenu", blackScreen));
            enabled = false;
            GetComponent<WorldSelection>().enabled = true;
            sceneManagementScript.LoadScene("WorldSelection");
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            indicators[selected].SetActive(false);
            selected = (selected + 2) % 3;
            indicators[selected].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            indicators[selected].SetActive(false);
            selected = (selected + 1) % 3;
            indicators[selected].SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectionCoroutine != null)
            {
                StopCoroutine(selectionCoroutine);
                StartCoroutine(selectLevel());
            }
            StartCoroutine(selectLevel());
        }
    }
    
    private IEnumerator selectLevel()
    {
        //text.SetActive(false);
        float alpha = 0f;
        float timer = 0f;
        while (alpha < 1f)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(alpha, 1f, timer / 3f);
            //Debug.Log(alpha);
            cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, planets[selected].transform.position, timer / 50f);
            Color currColor = blackScreen.color;
            currColor.a = alpha;
            blackScreen.color = currColor;
            yield return new WaitForFixedUpdate();
        }
        enabled = false;
        controlScript.planet = planets[selected];
        GetComponent<InLevelControl>().enabled = true;
        sceneManagementScript.LoadScene("LevelScene");
    }

}
