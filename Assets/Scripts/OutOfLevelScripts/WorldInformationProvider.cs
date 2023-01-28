using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldInformationProvider : MonoBehaviour
{
    [SerializeField] private Transform[] planetTransforms = new Transform[4];
    [SerializeField] private GameObject planetCenter;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] public RawImage blackScreen;
    [SerializeField] private GameObject[] text;

    private WorldSelection wSelection;
    private Coroutine startingCoroutine;
    void Awake()
    {
        startingCoroutine = StartCoroutine(TryUpdateGameMaster());
    }
    
    IEnumerator TryUpdateGameMaster()
    {
        while (wSelection == null)
        {
            wSelection = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<WorldSelection>();
            yield return new WaitForFixedUpdate();
        }

        wSelection.planetTransforms = planetTransforms;
        wSelection.planetCenter = planetCenter;
        wSelection.cameraObject = cameraObject;
        wSelection.blackScreen = blackScreen;
        wSelection.text = text;
        StopCoroutine(startingCoroutine);
    }
}
