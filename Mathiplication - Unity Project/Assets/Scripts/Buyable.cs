using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyable : MonoBehaviour
{
    [SerializeField] GameMaster gameMaster;
    [SerializeField] public int ThisItemCost;
    [SerializeField] public int NumberSold;
    [SerializeField] public float PriceIncreaseFactor;


    public void BuyThisItem()
    {
        gameMaster.MeatballCount -= ThisItemCost;
        //Register item on the saveState
        //Increase next cost? Or already have that mapped out so they can buy multiple? and it needs to remember new cost on load


    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
