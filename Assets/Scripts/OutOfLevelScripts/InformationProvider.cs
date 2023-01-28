using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationProvider : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletedUI;
    [SerializeField] private GameObject youDiedUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject planetCenter;
    [SerializeField] private GameObject shipPosition;

    [SerializeField] private Button tryAgainButton;

    private Coroutine startingCoroutine;
    private InLevelControl _levelControl;
    private void Awake()
    {
        startingCoroutine = StartCoroutine(TryGetGameMaster());
        tryAgainButton.onClick.AddListener(delegate { _levelControl.RestartLevel();});
    }

    IEnumerator TryGetGameMaster()
    {
        while (_levelControl == null)
        {
            _levelControl = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<InLevelControl>();
            yield return new WaitForFixedUpdate();
        }
        _levelControl.levelCompletedUI = levelCompletedUI;
        _levelControl.youDiedUI = youDiedUI;
        _levelControl.pauseUI = pauseUI;
        _levelControl.planetPosition = planetCenter;
        _levelControl.shipPosition = shipPosition;
        _levelControl.StartLevel();
        StopCoroutine(startingCoroutine);
    }
}
