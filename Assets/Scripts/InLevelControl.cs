using System.Collections;
using TMPro;
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
    public TMP_Text [] text; //0 -> time | 1 -> Destroyed Projectiles | 2 -> Vulnerabilities | 3 -> Points

    private IGun gunScript;
    private IShip shipScript; 
    private ITrinket trinketScript;
    //                                      booleans for controlling the gameState
    public bool levelCompleted = false;
    public bool died = false;
    public bool autoRestart = false;
    private bool started = false;
    public bool waited = false;
    private bool firing = false;
    public bool isInhibited = false;
    public bool isPaused = false;
    //                                      misc
    [SerializeField] private float rotationSpeed;
    private float timer = 0.0f;
    private float points = 0.0f;
    public float destroyedProjectiles = 0.0f;
    public float hitVulnerabilities = 0.0f;
    private Vector3 rotationDirection;
    private Coroutine waitRoutine = null;

    //                                      references
    private Planet planetScript;
    public SceneManagement.GameStates gameState;
    public SceneManagement sceneManagementScript;

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
        instantiatedPlanet = Instantiate(planet);
        instantiatedPlanet.transform.position = planetPosition.transform.position;
        instantiatedPlanet.transform.localScale = planetPosition.transform.localScale;
        instantiatedShip = Instantiate(ships[currentShipIndex]);
        shipScript = instantiatedShip.GetComponent<IShip>();
        shipScript.tryGetInfo();
        instantiatedShip.transform.position = shipPosition.transform.position;
        instantiatedShip.transform.localScale = shipPosition.transform.localScale;
        instantiatedTrinket = Instantiate(trinkets[currentTrinketIndex], instantiatedShip.transform);
        instantiatedTrinket.transform.position = instantiatedShip.transform.position;
        trinketScript = instantiatedTrinket.GetComponent<ITrinket>();
        instantiatedGun = Instantiate(guns[currentGunIndex], instantiatedShip.transform.GetChild(0));
        gunScript = instantiatedGun.GetComponent<IGun>();
        started = true;
    }

    void Update()
    {
        if (!started) return;
        if (gameState == SceneManagement.GameStates.InLevel)
        {
            if (CheckExceptions()) return;
            timer += Time.deltaTime;
            CheckInputs();
            if (isPaused) return;
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
                points = Mathf.Round(timer + destroyedProjectiles + hitVulnerabilities * 10);
                text[0].text += Mathf.Round(timer) + "s";
                text[1].text += " " + destroyedProjectiles;
                text[2].text += " " + hitVulnerabilities;
                text[3].text += "\n" + points;
            }
            if (!waited) return true;
            if (Input.anyKeyDown)
            {
                isPaused = false;
                isInhibited = false;
                died = false;
                firing = false;
                LoadWorldSelection();
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

        if (isInhibited)
        {
            return true;
        }
        
        return false;
    }


    public void LoadWorldSelection()
    {
        waited = false;
        enabled = false;
        levelCompleted = false;
        var ws = gameObject.GetComponent<WorldSelection>();
        ws.enabled = true;
        ws.worldIndex = 0;
        sceneManagementScript.LoadScene("WorldSelection");
        
    }
    public void RestartLevel()
    {
        isPaused = false;
        isInhibited = false;
        died = false;
        firing = false;
        youDiedUI.SetActive(false);
        sceneManagementScript.LoadScene("LevelScene");
    }

    void CheckInputs()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //open the menu
            isPaused = !isPaused;
            pauseUI.SetActive(!pauseUI.activeSelf);
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            firing = false;
        }
        
        if (isPaused) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //fire your gun
            firing = true;
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

        
    }

    void rotatePlanet()
    {
        instantiatedPlanet.transform.Rotate(rotationSpeed * rotationDirection * Time.deltaTime);
    }

    IEnumerator waitABit()
    {
        yield return new WaitForSeconds(2);
        waited = true;
    }
}
