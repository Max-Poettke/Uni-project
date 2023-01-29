using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsLevel : MonoBehaviour
{
    public void ExitLevel()
    {
        InLevelControl controller = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
        controller.waited = false;
        controller.enabled = false;
        controller.levelCompleted = false;
        controller.gameObject.GetComponent<WorldSelection>().enabled = true;
        controller.sceneManagementScript.LoadScene("WorldSelection");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
