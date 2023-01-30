using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeep : MonoBehaviour
{
    public float points = 0;
    [SerializeField] public GameObject[] guns;
    private int currentGunIndex = 0;
    [SerializeField] public GameObject[] ships;
    private int currentShipIndex = 0;
    [SerializeField] public GameObject[] trinkets;
    private int currentTrinketIndex = 0;

    private InLevelControl controller;

    void Start()
    {
        controller = GetComponent<InLevelControl>();
    }
}
