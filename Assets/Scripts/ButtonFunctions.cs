using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("WorldSelection");
    }

    public void Options()
    {
    }

    public void Credits()
    {
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
