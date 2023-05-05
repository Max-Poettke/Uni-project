using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsLevel : MonoBehaviour
{
    [SerializeField] private GameObject[] uIs;
    public void ExitLevel()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameMaster");
        InLevelControl controller = gm.GetComponent<InLevelControl>();
        WorldSelection worldSelection = gm.GetComponent<WorldSelection>();
        controller.isPaused = false;
        controller.waited = false;
        controller.enabled = false;
        controller.points = 0;
        controller.levelCompleted = false;
        controller.died = false;
        controller.points = 0;
        for (var i = 0; i < uIs.Length; i++)
        {
            uIs[i].SetActive(false);
        }
        worldSelection.targetRotationY = 0;
        worldSelection.worldIndex = 0;
        worldSelection.enabled = true;
        controller.sceneManagementScript.LoadScene("WorldSelection");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
