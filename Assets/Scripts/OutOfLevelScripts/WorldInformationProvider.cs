using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WorldInformationProvider : MonoBehaviour
{
    [SerializeField] private Transform[] planetTransforms = new Transform[4];
    [SerializeField] private GameObject planetCenter;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] public RawImage blackScreen;
    [SerializeField] private GameObject[] text;

    private WorldSelection wSelection;
    private Coroutine startingCoroutine;
    
    //ShopKeeping
    
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject purchaseUI;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] public GameObject[] ships;
    [SerializeField] public GameObject[] guns;
    [SerializeField] public GameObject[] trinkets;
    [SerializeField] private float [] prices;
    public GameObject[] textObjects;
    private float currentPrice;
    private int currentShipIndex = 0;
    private int currentGunIndex = 0;
    private int currentTrinketIndex = 0;
    private InLevelControl controller;
    private ShopKeep shopKeep;
    void Awake()
    {
        var gm = GameObject.FindGameObjectWithTag("GameMaster");
        shopKeep = gm.GetComponent<ShopKeep>();
        controller = gm.GetComponent<InLevelControl>();
        if (!shopKeep.initialized)
        {
            // Initialize Shop for currently available items
            shopKeep.purchasedItems[0] = new bool[ships.Length];
            shopKeep.purchasedItems[1] = new bool[guns.Length];
            shopKeep.purchasedItems[2] = new bool[trinkets.Length];
            shopKeep.purchasedItems[0][0] = true;
            shopKeep.purchasedItems[1][0] = true;
            prices = new float [Mathf.Max(Mathf.Max(guns.Length, ships.Length), trinkets.Length)];
            for (int i = 0; i < prices.Length; i++)
            {
                prices[i] = 700 + 700 * i;
            }
        }
        
        startingCoroutine = StartCoroutine(TryUpdateGameMaster());
        SetText(textObjects[0].GetComponent<TMP_Text>(), ships[currentShipIndex].name, true);
        SetText(textObjects[1].GetComponent<TMP_Text>(), guns[currentGunIndex].name, true);
        SetText(textObjects[2].GetComponent<TMP_Text>(), trinkets[currentTrinketIndex].name, shopKeep.purchasedItems[2][0]);
    }

    public void Update()
    {
        currentPrice = 0;
        if (shopKeep.purchasedItems[0][currentShipIndex] == false)
        {
            currentPrice += prices[currentShipIndex];
        }

        if (shopKeep.purchasedItems[1][currentGunIndex] == false)
        {
            currentPrice += prices[currentGunIndex];
        }

        if (shopKeep.purchasedItems[2][currentTrinketIndex] == false)
        {
            currentPrice += prices[currentTrinketIndex];
        }

        priceText.text = "Price: " + currentPrice;
        pointsText.text = "Points: " + shopKeep.points;
    }
    
    IEnumerator TryUpdateGameMaster()
    {
        while (wSelection == null)
        {
            GameObject gm = GameObject.FindGameObjectWithTag("GameMaster"); 
            wSelection = gm.GetComponent<WorldSelection>();
            shopKeep = gm.GetComponent<ShopKeep>();
            yield return new WaitForFixedUpdate();
        }

        wSelection.planetTransforms = planetTransforms;
        wSelection.planetCenter = planetCenter;
        wSelection.cameraObject = cameraObject;
        wSelection.blackScreen = blackScreen;
        wSelection.text = text;
        wSelection.shopUI = shopUI;
        
        StopCoroutine(startingCoroutine);
    }

    private void SetText(TMP_Text textObj, string value, bool unlocked)
    {
        textObj.text = value;
        textObj.color = Color.white;
        if (unlocked) return;
        textObj.color = Color.red;
    }
    
    public void ShipSwapLeft()
    {
        currentShipIndex = (currentShipIndex + ships.Length - 1) % ships.Length;
        SetText(textObjects[0].GetComponent<TMP_Text>(), ships[currentShipIndex].name, shopKeep.purchasedItems[0][currentShipIndex]);
        if (shopKeep.purchasedItems[0][currentShipIndex] == false)
        {
            return;
        }
        controller.ship = ships[currentShipIndex];
    }

    public void ShipSwapRight()
    {
        currentShipIndex = (currentShipIndex + 1) % ships.Length;
        SetText(textObjects[0].GetComponent<TMP_Text>(), ships[currentShipIndex].name, shopKeep.purchasedItems[0][currentShipIndex]);
        if (shopKeep.purchasedItems[0][currentShipIndex] == false)
        {
            return;
        }
        controller.ship = ships[currentShipIndex];
    }

    public void GunSwapLeft()
    {
        currentGunIndex = (currentGunIndex + guns.Length - 1) % guns.Length;
        SetText(textObjects[1].GetComponent<TMP_Text>(), guns[currentGunIndex].name, shopKeep.purchasedItems[1][currentGunIndex]);
        if (shopKeep.purchasedItems[1][currentGunIndex] == false)
        {
            return;
        }
        controller.gun = guns[currentGunIndex];
    }

    public void GunSwapRight()
    {
        currentGunIndex = (currentGunIndex + 1) % guns.Length;
        SetText(textObjects[1].GetComponent<TMP_Text>(), guns[currentGunIndex].name, shopKeep.purchasedItems[1][currentGunIndex]);
        if (shopKeep.purchasedItems[1][currentGunIndex] == false)
        {
            return;
        }
        controller.gun = guns[currentGunIndex];
    }

    public void TrinketsSwapLeft()
    {
        currentTrinketIndex = (currentTrinketIndex + trinkets.Length - 1) % trinkets.Length;
        SetText(textObjects[2].GetComponent<TMP_Text>(), trinkets[currentTrinketIndex].name, shopKeep.purchasedItems[2][currentTrinketIndex]);
        if (shopKeep.purchasedItems[2][currentTrinketIndex] == false)
        {
            return;
        }
        controller.trinket = trinkets[currentTrinketIndex];
    }

    public void TrinketsSwapRight()
    {
        currentTrinketIndex = (currentTrinketIndex + 1) % trinkets.Length;
        SetText(textObjects[2].GetComponent<TMP_Text>(), trinkets[currentTrinketIndex].name, shopKeep.purchasedItems[2][currentTrinketIndex]);
        if (shopKeep.purchasedItems[2][currentTrinketIndex] == false)
        {
            return;
        }
        controller.trinket = trinkets[currentTrinketIndex];
    }

    public void PurchaseItem()
    {
        if (currentPrice > shopKeep.points) return;
        shopKeep.purchasedItems[0][currentShipIndex] = true;
        shopKeep.purchasedItems[1][currentGunIndex] = true;
        shopKeep.purchasedItems[2][currentTrinketIndex] = true;
        shopKeep.points -= currentPrice;
        SetText(textObjects[0].GetComponent<TMP_Text>(), ships[currentShipIndex].name, shopKeep.purchasedItems[0][currentShipIndex]);
        SetText(textObjects[1].GetComponent<TMP_Text>(), guns[currentGunIndex].name, shopKeep.purchasedItems[1][currentGunIndex]);
        SetText(textObjects[2].GetComponent<TMP_Text>(), trinkets[currentTrinketIndex].name, shopKeep.purchasedItems[2][currentTrinketIndex]);
        controller.ship = ships[currentShipIndex];
        controller.gun = guns[currentGunIndex];
        controller.trinket = trinkets[currentTrinketIndex];
    }
}
