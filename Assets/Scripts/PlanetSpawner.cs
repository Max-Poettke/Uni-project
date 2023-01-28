using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public static GameObject[] positions;
    private LevelSelection lSel;
    // create reference to levelselection and get set the indicators to the indicators of the new instances

    void Awake()
    {
        positions = GameObject.FindGameObjectsWithTag("PlanetPosition");
        lSel = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<LevelSelection>();
        SpawnPlanets();
    }

    void SpawnPlanets()
    {
        
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject inst = Instantiate(lSel.planets[i], positions[i].transform);
            inst.transform.position = positions[i].transform.position;
            inst.GetComponent<Rotatator>().enabled = true;
        }
    }
}
