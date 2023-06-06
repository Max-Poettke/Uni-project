using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class WorldInformationProvider : MonoBehaviour
{
    [SerializeField] private Transform[] planetTransforms = new Transform[4];
    [SerializeField] private GameObject planetCenter;
    [SerializeField] private GameObject cameraObject;
    [SerializeField] public RawImage blackScreen;
    [SerializeField] private GameObject[] text;
    [SerializeField] private TMP_Text[] upgradeText;

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
    private bool firstEver = true;
    public GameObject[] textObjects;
    private float currentPrice;
    public int currentShipIndex = 0;
    public int currentGunIndex = 0;
    
    public int dmgUpgrade = 0;
    public int firingUpgrade = 0;
    public int penetrationUpgrade = 0;
    public float upgradePrice = 0;

    private IGun gunScript;
    
    [SerializeField] public int currentTrinketIndex = 0;
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
            shopKeep.initialized = true;
        }
        prices = new float [Mathf.Max(Mathf.Max(guns.Length, ships.Length), trinkets.Length)];
        for (int i = 0; i < prices.Length; i++)
        {
            prices[i] = 700 + 700 * i;
        }
    
        
        
        if (firstEver)
        {
            foreach (var gun in guns)
            {
                gunScript = gun.GetComponent<IGun>();
                dmgUpgrade = 0;
                firingUpgrade = 0;
                penetrationUpgrade = 0;
                gunScript.Upgrade(dmgUpgrade - gunScript.dmgUpgrade, penetrationUpgrade - gunScript.penetrationUpgrade, firingUpgrade - gunScript.firingUpgrade);
                gunScript.dmgUpgrade = 0;
                gunScript.firingUpgrade = 0;
                gunScript.penetrationUpgrade = 0;
            }
        }

        gunScript = guns[currentGunIndex].GetComponent<IGun>();
        startingCoroutine = StartCoroutine(TryUpdateGameMaster());
        SetText(textObjects[0].GetComponent<TMP_Text>(), ships[currentShipIndex].name, true);
        SetText(textObjects[1].GetComponent<TMP_Text>(), guns[currentGunIndex].name, true);
        SetText(textObjects[2].GetComponent<TMP_Text>(), trinkets[currentTrinketIndex].name, shopKeep.purchasedItems[2][0]);
        
        UpdateText();
    }

    public void UpdateText()
    {
        gunScript = guns[currentGunIndex].GetComponent<IGun>();
        dmgUpgrade = gunScript.dmgUpgrade;
        penetrationUpgrade = gunScript.penetrationUpgrade;
        firingUpgrade = gunScript.firingUpgrade;
        
        upgradeText[0].text = dmgUpgrade.ToString();
        upgradeText[1].text = penetrationUpgrade.ToString();
        upgradeText[2].text = firingUpgrade.ToString();
        
        SetText(upgradeText[0], dmgUpgrade.ToString(), true);
        SetText(upgradeText[1], penetrationUpgrade.ToString(), true);
        SetText(upgradeText[2], firingUpgrade.ToString(), true);
    }

    public void Update()
    {
        if (gunScript.dmgUpgrade == dmgUpgrade
            && gunScript.penetrationUpgrade == penetrationUpgrade
            && gunScript.firingUpgrade == firingUpgrade)
        {
            upgradePrice = 0;
        }
     
        currentPrice = upgradePrice;
        
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
        UpdateText();
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
        UpdateText();
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

    public void WantUpgradeDamage()
    {
        dmgUpgrade++;
        SetText(upgradeText[0], dmgUpgrade.ToString(), false);
        upgradePrice += 200;
    }

    public void WantUpgradePenetration()
    {
        penetrationUpgrade++;
        SetText(upgradeText[1], penetrationUpgrade.ToString(), false);
        upgradePrice += 300;
    }

    public void WantUpgradeFiringSpeed()
    {
        firingUpgrade++;
        SetText(upgradeText[2], firingUpgrade.ToString(), false);
        upgradePrice += 200;
    }

    public void ResetCart()
    {
        dmgUpgrade = gunScript.dmgUpgrade;
        firingUpgrade = gunScript.firingUpgrade;
        penetrationUpgrade = gunScript.penetrationUpgrade;
        UpdateText();
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
        gunScript.Upgrade(dmgUpgrade - gunScript.dmgUpgrade, penetrationUpgrade - gunScript.penetrationUpgrade, firingUpgrade - gunScript.firingUpgrade);
        gunScript.dmgUpgrade = dmgUpgrade;
        gunScript.firingUpgrade = firingUpgrade;
        gunScript.penetrationUpgrade = penetrationUpgrade;
        controller.gun = guns[currentGunIndex];
        gunScript = guns[currentGunIndex].GetComponent<IGun>();
        UpdateText();
        controller.trinket = trinkets[currentTrinketIndex];
    }
}
