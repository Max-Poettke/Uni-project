using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSpawner : MonoBehaviour
{
    public static GameObject[] positions;
    private LevelSelection lSel;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private RawImage blackScreen;

    void Awake()
    {
        positions = GameObject.FindGameObjectsWithTag("PlanetPosition");
        lSel = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelSelection>();
        lSel.cameraObject = cameraObject;
        lSel.blackScreen = blackScreen;
        SpawnPlanets();
    }

    void SpawnPlanets()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject inst = Instantiate(lSel.planets[i], positions[i].transform);
            inst.transform.position = positions[i].transform.position;
            inst.GetComponent<Rotatator>().enabled = true;
            lSel.indicators[i] = inst.transform.GetChild(0).gameObject;
            lSel.indicators[i].SetActive(false);
        }
        lSel.indicators[0].SetActive(true);
    }
}
