using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Buyable : MonoBehaviour
{
    [SerializeField] public int ItemID;
    [SerializeField] GameMaster gameMaster;
    [SerializeField] public TMP_Text NumberSoldText;
    [SerializeField] TMP_Text NumberBuyableText;
    [SerializeField] public int ThisItemCost = 1;
    [SerializeField] public int NumberSold = 0;
    [SerializeField] public float PriceIncreaseFactor = .5f;
    [SerializeField] public float PackingSpeedIncreaseFactor = .3f;

    [SerializeField] public float ReadyToPackIncreaseFactor = .3f;

    public float PackingSpeedIncrease;
    public float ReadyToPackIncrease;

    public int TrackedSold
    {
        get { return NumberSold; }
        set
        {
            NumberSold = value;
            PackingSpeedIncrease = Mathf.Pow(NumberSold, 1 + PackingSpeedIncreaseFactor);
            ReadyToPackIncrease = Mathf.Pow(NumberSold, 1 + ReadyToPackIncreaseFactor);
            gameMaster.RegisterPurchase(ItemID, NumberSold);
        }
    }
    public void BuyThisItem()
    {
        if(Mathf.RoundToInt(ThisItemCost * Mathf.Pow(NumberSold, 1 + PriceIncreaseFactor)) < gameMaster.MeatballCount)
        {
            gameMaster.MeatballCount -= Mathf.RoundToInt(ThisItemCost * Mathf.Pow(NumberSold, 1 + PriceIncreaseFactor));
            gameMaster.MeatballsShippedCount -= Mathf.RoundToInt(ThisItemCost * Mathf.Pow(NumberSold, 1 + PriceIncreaseFactor));
            //Register item on the saveState
            NumberSold++;
            NumberSoldText.text = NumberSold.ToString("n0");
            /////gameMaster.SaveManager.ThisSaveState.ItemsSoldList[ItemID] = NumberSold;
            //Increase next cost? Or already have that mapped out so they can buy multiple? and it needs to remember new cost on load

            //Need to add exponential increase
            PackingSpeedIncrease = Mathf.Pow(NumberSold, 1 + PackingSpeedIncreaseFactor);
            ReadyToPackIncrease = Mathf.Pow(NumberSold, 1 + ReadyToPackIncreaseFactor);
            gameMaster.RegisterPurchase(ItemID, NumberSold);

        }

    }

    void Start()
    {
    }

    private void Awake()
    {

    }


    void Update()
    {
        
    }
}
