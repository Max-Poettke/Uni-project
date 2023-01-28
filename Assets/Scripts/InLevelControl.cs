using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class InLevelControl : MonoBehaviour
{
    //                                      things to instantiate upon level start
    public GameObject planet;
    [SerializeField] private GameObject[] guns;
    private int currentGunIndex = 0;
    [SerializeField] private GameObject[] ships;
    private int currentShipIndex = 0;
    [SerializeField] private GameObject[] trinkets;
    private int currentTrinketIndex = 0;
    
    //                                      things to get from the scene
    
    public GameObject levelCompletedUI;
    public GameObject youDiedUI;
    public GameObject pauseUI;

    private IGun gunScript;
    private IShip shipScript; 
    private ITrinket trinketScript;
    //                                      booleans for controlling the gameState
    public bool levelCompleted = false;
    public bool died = false;
    public bool autoRestart = false;
    //                                      misc
    [SerializeField] private float rotationSpeed;
    private float timer = 0.0f;
    private float points = 0.0f;
    private Vector3 rotationDirection;

    //                                      references
    private Planet planetScript;
    public SceneManagement.GameStates gameState;
    private SceneManagement sceneManagementScript;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        gameState = SceneManagement.GameStates.InLevel;
        rotationDirection = transform.forward;
        gunScript = guns[currentGunIndex].GetComponent<IGun>();
        shipScript = ships[currentShipIndex].GetComponent<IShip>();
        trinketScript = trinkets[currentTrinketIndex].GetComponent<ITrinket>();
    }

    void Update()
    {
        if (gameState == SceneManagement.GameStates.InLevel)
        {
            if (CheckExceptions()) return;
            timer += Time.deltaTime;
            CheckInputs();
            rotatePlanet();
            shipScript.Move();
        }
    }

    void InitializeLevel()
    {
        
    }

    bool CheckExceptions()
    {
        if (levelCompleted)
        {
            //load level information UI
            levelCompletedUI.SetActive(true);
            return true;
        }

        if (died)
        {
            //Reload the level and reset important information
            if (autoRestart)
            {
                StartCoroutine(ResetLevel());   
            }
            youDiedUI.SetActive(true);
            return true;
        }
        return false;
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //fire your gun
            gunScript.Fire();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            //use your trinket
            trinketScript.Use();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //open the menu
            pauseUI.SetActive(true);
        }
    }

    void rotatePlanet()
    {
        planet.transform.Rotate(rotationSpeed * rotationDirection * Time.deltaTime);
    }

    public void RestartLevel()
    {
        
    }
    IEnumerator ResetLevel()
    {
        yield return null;
    }
}
