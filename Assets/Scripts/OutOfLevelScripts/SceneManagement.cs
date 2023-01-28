using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    private static GameObject sceneManagerInstance;
    private GameObject _camera;
    private InLevelControl controller;
    public enum GameStates
    {
        MainMenu,
        WorldSelection,
        LevelSelection,
        ShipModification,
        LevelScene,
        InLevel
    }
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (sceneManagerInstance == null)
        {
            sceneManagerInstance = this.gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
        _camera = GameObject.FindGameObjectWithTag("Camera");
        controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
    }

    public void LoadScene(string sceneName)
    {
        //0 = MainMenu
        //1 = WorldSelection
        //2 = LevelSelection
        //3 = ShipModification
        //4 = LevelScene
        
        Debug.Log("Loading scene: " + sceneName);
        controller.gameState = (GameStates) System.Enum.Parse(typeof(GameStates), sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator LeaveScene(string targetScene, RawImage bImage)
    {
        float alpha = 0f;
        float timer = 0f;
        Debug.Log("Leaving to scene: " + targetScene);
        while (alpha < 1f)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(alpha, 1f, timer / 3f);
            Debug.Log(alpha);
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, 
                new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z - 5f), timer / 50f);
            Color currColor = bImage.color;
            currColor.a = alpha;
            bImage.color = currColor;
            yield return new WaitForFixedUpdate();
        }

        controller.gameState = (GameStates) System.Enum.Parse(typeof(GameStates), targetScene);
        LoadScene(targetScene);
    }
}
