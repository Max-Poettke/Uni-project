using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Experimental.Audio;
using UnityEngine.UI;

public class InLevelControl : MonoBehaviour
{
    
    public bool cantMove = false;
    
    [SerializeField] public GameObject planet;
    [SerializeField] private GameObject currentGun;
    [SerializeField] private GameObject currentShip;
    //[SerializeField] private GameObject currentTrinket;
    
    [SerializeField] private float rotationSpeed;

    [SerializeField] private GameObject bgImage;
    [SerializeField] private GameObject text;

    private IGun gunScript;
    private IShip shipScript;   
    //private ITrinket trinketScript;
    
    private float timer = 0;
    private Vector3 rotationDirection;
    //private Coroutine shrinkingRoutine;
    private Coroutine endRoutine;
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
        cantMove = false;
        rotationDirection = transform.forward;
        gunScript = currentGun.GetComponent<IGun>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (gameState == SceneManagement.GameStates.InLevel)
        {
            if (cantMove)
            {
                //load level information UI
                bgImage.SetActive(true);
                text.SetActive(true);
                return;
            }
            timer += Time.deltaTime;
            CheckInputs();
            rotatePlanet();    
        }
        
        if (cantMove) return;
        shipScript.Move();
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shrinkingRoutine == null)
            {
                shrinkingRoutine = StartCoroutine(planetScript.ShrinkPlanet(50));
                return;
            }
            StopCoroutine(shrinkingRoutine);
            shrinkingRoutine = StartCoroutine(planetScript.ShrinkPlanet(50));
        }
        */
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //fire your gun
            gunScript.Fire();
            /*
            laser.enabled = true;
            laser.Play();
            firing = true;
            scaleRoutine = StartCoroutine(AnimateLaser());
            */
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            //use your utility
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //open the menu
        }
    }

    void rotatePlanet()
    {
        if (cantMove) return;
        planet.transform.Rotate(rotationSpeed * rotationDirection * Time.deltaTime);
    }
}
