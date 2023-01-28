using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] private GameObject controlsUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            controlsUI.SetActive(false);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("WorldSelection");
    }

    public void Controls()
    {
        controlsUI.SetActive(true);
    }

    public void Credits()
    {
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
