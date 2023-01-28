using UnityEngine;

public class StandardShip : MonoBehaviour, IShip
{
    private Camera _camera;
    public Transform planet;
    public InLevelControl inLevelControlScript;

    private bool foundTargetPlanet = false;

    [SerializeField]
    private float speed = 9.0f;
    [SerializeField] 
    private float rotationSpeed = 30f;
    public bool isMoving = false;

    public void Move()
    {
        if (!foundTargetPlanet)
        {
            tryGetInfo();
            return;
        }
        
        transform.LookAt(planet);
        transform.Rotate(Vector3.up, -90);
        transform.Rotate(Vector3.back, 90);
        float amtToMoveX = 0;
        float amtToMoveY = 0;
        isMoving = true;
        if (Input.GetAxis("Horizontal") > 0)
        {
            amtToMoveX = Time.deltaTime * speed;
            
        } else if (Input.GetAxis("Horizontal") == 0)
        {
            if (Input.GetAxis("Vertical") == 0)
            {
                isMoving = false;
            }
            amtToMoveX = 0;
        }
        else
        {
            amtToMoveX = -Time.deltaTime * speed;
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            amtToMoveY = Time.deltaTime * rotationSpeed;
        } else if (Input.GetAxis("Vertical") == 0)
        {
            amtToMoveY = 0;
        } else
        {
            amtToMoveY = -Time.deltaTime * rotationSpeed;
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.position += transform.up * amtToMoveX;
        transform.RotateAround(planet.position, planet.forward, -amtToMoveY);
        if (!CheckWithinBounds(amtToMoveX, amtToMoveY)) transform.position = pos;
    }

    public void Die()
    {
        inLevelControlScript.died = true;
        Destroy(gameObject);
    }

    public void tryGetInfo()
    {
        if (inLevelControlScript == null)
        {
            inLevelControlScript = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
        }
        
        planet = inLevelControlScript.planet.transform;
        _camera = Camera.main;
        foundTargetPlanet = true;
    }

    private bool CheckWithinBounds(float xMove, float yMove)
    {
        bool result = true;
        Vector3 posInViewSpace = _camera.WorldToViewportPoint(transform.position + transform.up * xMove);
        if (posInViewSpace.x < 0.0f
            ||posInViewSpace.x > 0.65f
            ||posInViewSpace.y < 0.0f
            ||posInViewSpace.y > 1.0f)
        {
            result = false;
        }
        return result;
    }
    
    /*
    private IEnumerator DestroyShip()
    {
        // Destroy animation
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        GetComponent<Renderer>().enabled = false;

        if (lives > 0)
        {
            // reset ship to camera center  and move in 
            var startPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, -0.1f, 0));
            transform.position = new Vector3(startPosition.x,startPosition.y, transform.position.z);
            yield return new WaitForSeconds(3);
            playerState = state.Invincible;
            GetComponent<Renderer>().enabled = true;
            var blinkRoutine = StartCoroutine(Blink());
            
            // check  for good position in camera view
            var positonInView = Camera.main.WorldToViewportPoint(transform.position);
            while (positonInView.y <=  0.45)
            {
                // move ship upwards until vincible is over
                transform.Translate(Time.deltaTime*speed*0.3f*Vector3.up);
                yield return null;
                positonInView = Camera.main.WorldToViewportPoint(transform.position);
            }

            // start play phase again and stop blinking
            playerState = state.Playing;
            StopCoroutine(blinkRoutine);
            // make sure  renderer is  active
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            SceneManager.LoadScene("lose");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy && playerState == state.Playing)
        {
            // reduce life if we got hit  by an   enemy
            lives--;
            playerState = state.Explosion;
            StartCoroutine(DestroyShip());
        }
    }
    */
}
