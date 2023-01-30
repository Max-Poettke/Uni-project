using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationProvider : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletedUI;
    [SerializeField] private GameObject youDiedUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject planetCenter;
    [SerializeField] private GameObject shipPosition;
    [SerializeField] private TMP_Text[] text;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text pointText;

    [SerializeField] private Button tryAgainButton;

    private Coroutine startingCoroutine;
    private InLevelControl _levelControl;
    private IPlanet planetControl;
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
        _levelControl.text = text;
        _levelControl.slider = slider;
        _levelControl.pointText = pointText;
        _levelControl.StartLevel();
        StopCoroutine(startingCoroutine);
    }
}
