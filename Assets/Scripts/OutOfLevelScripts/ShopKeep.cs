using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopKeep : MonoBehaviour
{
    public float points = 0;
    //2 dimensional Array -> 3 types ship, gun and trinket with respective isPurchased values
    public bool[][] purchasedItems = new bool[3][];
    public bool initialized = false;
}
