using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private RawImage blackScreen;
    private SceneManagement sceneManagementScript;
    private int selected = 0;
    public GameObject [] planets;
    public GameObject[] indicators;
    private Coroutine selectorBlinkRoutine;
    

    void Start()
    {
        sceneManagementScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
        for (int i = 0; i < planets.Length; i++)
        {
            indicators[i] = planets[i].transform.GetChild(0).gameObject;
            indicators[i].SetActive(false);
        }
        
        //indicators[0].SetActive(true);
        //      blackScreeen can for some reason not be set. -> GameObject reference is missing
        // this is a test
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
            sceneManagementScript.LoadScene("WorldSelection");
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            indicators[selected].SetActive(false);
            selected = (selected + 1) % 3;
            indicators[selected].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            indicators[selected].SetActive(false);
            selected = (selected + 2) % 3;
            indicators[selected].SetActive(true);
        }
    }

}
