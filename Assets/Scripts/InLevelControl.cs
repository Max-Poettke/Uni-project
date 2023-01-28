using System.Collections;
using UnityEngine;

public class InLevelControl : MonoBehaviour
{
    private static GameObject gameMasterInstance;
    //                                      things to instantiate upon level start
    public GameObject planet;
    [SerializeField] private GameObject[] guns;
    private int currentGunIndex = 0;
    [SerializeField] private GameObject[] ships;
    private int currentShipIndex = 0;
    [SerializeField] private GameObject[] trinkets;
    private int currentTrinketIndex = 0;
    
    public GameObject instantiatedPlanet;
    public GameObject instantiatedShip;
    public GameObject instantiatedGun;
    public GameObject instantiatedTrinket;
    
    //                                      things to get from the scene
    public GameObject shipPosition;
    public GameObject planetPosition;
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
    private bool started = false;
    private bool waited = false;
    private bool firing = false;
    //                                      misc
    [SerializeField] private float rotationSpeed;
    private float timer = 0.0f;
    private float points = 0.0f;
    private Vector3 rotationDirection;
    private Coroutine waitRoutine = null;

    //                                      references
    private Planet planetScript;
    public SceneManagement.GameStates gameState;
    private SceneManagement sceneManagementScript;

    void Awake()
    {
        DontDestroyOnLoad(this); 
        if (gameMasterInstance == null)
        {
            gameMasterInstance = this.gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
            
    }

    public void StartLevel()
    {
        gameState = SceneManagement.GameStates.InLevel;
        sceneManagementScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagement>();
        rotationDirection = transform.forward;
        gunScript = guns[currentGunIndex].GetComponent<IGun>();
        //shipScript = ships[currentShipIndex].GetComponent<IShip>();
        instantiatedPlanet = Instantiate(planet);
        instantiatedPlanet.transform.position = planetPosition.transform.position;
        instantiatedPlanet.transform.localScale = planetPosition.transform.localScale;
        instantiatedShip = Instantiate(ships[currentShipIndex]);
        instantiatedPlanet.GetComponent<Rotatator>().enabled = true;
        shipScript = instantiatedShip.GetComponent<IShip>();
        shipScript.tryGetInfo();
        instantiatedShip.transform.position = shipPosition.transform.position;
        instantiatedShip.transform.localScale = shipPosition.transform.localScale;
        instantiatedGun = Instantiate(guns[currentGunIndex], instantiatedShip.transform.GetChild(0));
        gunScript = instantiatedGun.GetComponent<IGun>();
        started = true;
        //trinketScript = trinkets[currentTrinketIndex].GetComponent<ITrinket>();
    }

    void Update()
    {
        if (!started) return;
        if (gameState == SceneManagement.GameStates.InLevel)
        {
            if (CheckExceptions()) return;
            timer += Time.deltaTime;
            CheckInputs();
            rotatePlanet();
            shipScript.Move();
        }
    }

    bool CheckExceptions()
    {
        if (levelCompleted)
        {
            //load level information UI
            levelCompletedUI.SetActive(true);
            if (waitRoutine == null)
            {
                waitRoutine = StartCoroutine(waitABit());    
            }
            if (!waited) return true;
            if (Input.anyKeyDown)
            {
                waited = false;
                enabled = false;
                levelCompleted = false;
                gameObject.GetComponent<WorldSelection>().enabled = true;
                sceneManagementScript.LoadScene("WorldSelection");
            }
            return true;
        }

        if (died)
        {
            //Reload the level and reset important information
            if (autoRestart)
            {
                sceneManagementScript.LoadScene("LevelScene");
            }
            youDiedUI.SetActive(true);
            return true;
        }
        return false;
    }

    public void RestartLevel()
    {
        died = false;
        firing = false;
        youDiedUI.SetActive(false);
        sceneManagementScript.LoadScene("LevelScene");
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //fire your gun
            firing = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            firing = false;
        }

        if (firing)
        {
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

    IEnumerator waitABit()
    {
        yield return new WaitForSeconds(2);
        waited = true;
    }
}
