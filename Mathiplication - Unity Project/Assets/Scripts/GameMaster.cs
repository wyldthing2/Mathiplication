using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//set packingmax when item is sold
//make multipliers work
//load startup sets the progressbar max correctly

[System.Serializable] public class ItemsToBuy
{ 
    [SerializeField] public Buyable BuyableItem;
    [SerializeField] public int NumberSold;
    public float ItemSpeedBoast;
    public float ItemPackableBoost;
}

public class GameMaster : MonoBehaviour
{
    [SerializeField] public float GoalIncreaseRate = .2f;

    [SerializeField] public SaveManager SaveManager;

    [SerializeField] GameObject meatballSource;
    [SerializeField] LaunchMeatballs meatballLauncher;

    [SerializeField] TMP_Text FirstNumber;
    [SerializeField] TMP_Text SecondNumber;
    static public GameMaster TheGameMaster;
    [SerializeField] int num1;
    [SerializeField] int num2;

    [SerializeField] public int MeatballCount = 0;
    [SerializeField] public int MeatballsShippedCount = 0;
    [SerializeField] float accummulatingMeatball = 0;

    //For Upgrades
    [SerializeField] public float MeatballPackingPerSecond = .01f;
    [SerializeField] public float MeatballsPerAnswer = 1;
    float BaselineMeatballPackingPerSecond;
    float BaselineMeatballsPerAnswer;
    float totalPackingSpeedFactor = 1;
    float totalPackableFactor = 1;


    [SerializeField] public int CurrentGoal = 100;
    [SerializeField] public float CurrentGoalProgress = 0;
    [SerializeField] float CurrentGoalWorkInProgress = 0;
    [SerializeField] TMP_Text meatballCountDisplay;
    [SerializeField] ProgressTracker myProgressTracker;
    [SerializeField] Slider myProgressTrackerWorkInProgress;
    [SerializeField] public float MeatballShippingTime = .1f;




    [SerializeField] GameObject CorrectSplosion;
    [SerializeField] GameObject NotRightSplosion;

    [SerializeField] public List<ItemsToBuy> ItemsBoughtList = new List<ItemsToBuy>();
    //attributes from each? That takes every calculation


    // Start is called before the first frame update
    void Awake()
    {
        TheGameMaster = this;
        BaselineMeatballPackingPerSecond = MeatballPackingPerSecond;
        BaselineMeatballsPerAnswer = MeatballsPerAnswer;
        setNumbers();
        SaveManager.Load();
        if (SaveManager.ThisSaveState.ItemsSoldList.Count < ItemsBoughtList.Count)
        {
            SaveManager.ThisSaveState.ItemsSoldList.Capacity = 1;
                //ItemsBoughtList.Count;

        }
        MeatballCount = SaveManager.ThisSaveState.MeatballCount;
        meatballCountDisplay.text = MeatballCount.ToString("n0");
        CurrentGoal = SaveManager.ThisSaveState.CurrentGoal;
        CurrentGoalProgress = (float)MeatballCount / (float)CurrentGoal;
        myProgressTracker.SetProgressBar(CurrentGoalProgress);
        MeatballsShippedCount = SaveManager.ThisSaveState.MeatballsShippedCount;
        CurrentGoalWorkInProgress = (float)MeatballsShippedCount/(float)CurrentGoal;
        myProgressTrackerWorkInProgress.value = CurrentGoalWorkInProgress;

        for (int i = 0; i < SaveManager.ThisSaveState.ItemsSoldList.Count; i++)
        {
            ItemsBoughtList[i].BuyableItem.ItemID = i;
            ItemsBoughtList[i].NumberSold = SaveManager.ThisSaveState.ItemsSoldList[i];
            ItemsBoughtList[i].BuyableItem.TrackedSold = ItemsBoughtList[i].NumberSold;
            ItemsBoughtList[i].BuyableItem.NumberSoldText.text = ItemsBoughtList[i].BuyableItem.NumberSold.ToString("n0");

            totalPackableFactor = totalPackableFactor * ItemsBoughtList[i].BuyableItem.ReadyToPackIncreaseFactor;
            totalPackingSpeedFactor = totalPackingSpeedFactor * ItemsBoughtList[i].BuyableItem.PackingSpeedIncreaseFactor;
        }

        InvokeRepeating("ShipMeatballs", 0, MeatballShippingTime);

    }

    void setNumbers()
    {
        num1 = Mathf.RoundToInt(Random.value * 5);
        FirstNumber.text = num1.ToString();
        num2 = Mathf.RoundToInt(Random.value * 3);
        SecondNumber.text = num2.ToString();

    }

    //CheckAnswer Internal Functions 
    void DeactivateObjectCorrectSplosion()
    {
        CorrectSplosion.SetActive(false);
        
    }
    void DeactivateObjectNotRightSplosion()
    {
        NotRightSplosion.SetActive(false);

    }


    public void CalculateNewPackingAndPackableRate(int itemID, float packableFactor, float packingSpeedFactor)
    {
        

        MeatballPackingPerSecond = BaselineMeatballPackingPerSecond * totalPackingSpeedFactor;
        MeatballsPerAnswer = BaselineMeatballsPerAnswer * totalPackableFactor;
    }

    public void CheckAnswer(int answer)
    {
        Debug.Log(answer);
        if (answer == num1 * num2)
        {
            meatballLauncher.transform.position = meatballSource.transform.position;
            meatballLauncher.CommandLaunchXNumberOfMeatballsAtThisYTransform(num1 * num2, meatballLauncher.transform, myProgressTracker.ProgressBar.transform);

            MeatballCount += Mathf.RoundToInt(answer*MeatballsPerAnswer);
            if (MeatballCount > CurrentGoal)
            {
                CurrentGoal = Mathf.RoundToInt(Mathf.Pow(CurrentGoal, 1+ GoalIncreaseRate));
                SaveManager.ThisSaveState.CurrentGoal = CurrentGoal;
            }
            CurrentGoalProgress = (float)MeatballCount / (float)CurrentGoal;
            

            //if (CurrentGoalProgress > 1)
            //{
                //CurrentGoalProgress = 1;
            //}
            myProgressTracker.SetProgressBar(CurrentGoalProgress);

            //if (CurrentGoalProgress < 1)
            //{
                
            //}

            CorrectSplosion.SetActive(true);
            Invoke("DeactivateObjectCorrectSplosion", 1);
            //activate CORRECT
            Debug.Log("Progress " + CurrentGoalProgress);
            setNumbers();
        }
        else
        {
            //activate incorrect and make it fade out and shrink and lose points!
            Debug.Log("Wrong, " + num1*num2);
            MeatballCount -= Mathf.RoundToInt((float)MeatballCount * (float).5);
            if (MeatballCount < 0)
            {
                MeatballCount = 0;
            }
            CurrentGoalProgress = (float)MeatballCount / (float)CurrentGoal;
            myProgressTracker.SetProgressBar(CurrentGoalProgress);
            NotRightSplosion.SetActive(true);
            Invoke("DeactivateObjectNotRightSplosion", 1);


        }

        //update the save state script
        SaveManager.ThisSaveState.MeatballCount = MeatballCount;
        
        //run save()
        SaveManager.Save();
        
    }

    void ShipMeatballs()
    {
        if (MeatballsShippedCount < MeatballCount)
        {
            //add to meatball
            accummulatingMeatball += MeatballPackingPerSecond;
            Debug.Log("Accummulated: " + accummulatingMeatball);
            //is it greater than 1? Then Do it!
            if (accummulatingMeatball > 1)
            {
                MeatballsShippedCount += Mathf.RoundToInt(accummulatingMeatball);
                accummulatingMeatball = 0;
                CurrentGoalWorkInProgress = (float)MeatballsShippedCount / (float)CurrentGoal;
                myProgressTrackerWorkInProgress.value = CurrentGoalWorkInProgress;
                meatballCountDisplay.text = MeatballsShippedCount.ToString("n0");

                SaveManager.ThisSaveState.MeatballsShippedCount = MeatballsShippedCount;
            }
        }
        else if (MeatballsShippedCount > MeatballCount)
        {
            MeatballsShippedCount = MeatballCount;
            CurrentGoalWorkInProgress = (float)MeatballsShippedCount / (float)CurrentGoal;
            myProgressTrackerWorkInProgress.value = CurrentGoalWorkInProgress;
            meatballCountDisplay.text = MeatballsShippedCount.ToString("n0");

            SaveManager.ThisSaveState.MeatballsShippedCount = MeatballsShippedCount;
        }
    }

    public void RegisterPurchase(int itemID, int numberSold)
    {
        //Save the items sold so far


        if (SaveManager.ThisSaveState.ItemsSoldList.Count < ItemsBoughtList.Count)
        {
            SaveManager.ThisSaveState.ItemsSoldList.Clear();

            for (int i = 0; i < ItemsBoughtList.Count; i++)
            {
                SaveManager.ThisSaveState.ItemsSoldList.Add(ItemsBoughtList[i].NumberSold);
            }
            //ItemsBoughtList.Count;

        }
        
        //WORKING ON THIS
        SaveManager.ThisSaveState.ItemsSoldList[0] = ItemsBoughtList[itemID].NumberSold;
        

        totalPackableFactor = 1;
        totalPackingSpeedFactor = 1;

        ItemsBoughtList[itemID].NumberSold = numberSold;
        CurrentGoalWorkInProgress = (float)MeatballsShippedCount / (float)CurrentGoal;
        myProgressTrackerWorkInProgress.value = CurrentGoalWorkInProgress;
        meatballCountDisplay.text = MeatballsShippedCount.ToString("n0");
        CurrentGoalProgress = (float)MeatballCount / (float)CurrentGoal;
        myProgressTracker.SetProgressBar(CurrentGoalProgress);

        //calculate latest multipliers
        for (int i = 0; i < SaveManager.ThisSaveState.ItemsSoldList.Count; i++)
        {
            totalPackableFactor = totalPackableFactor * (1 + ItemsBoughtList[i].BuyableItem.ReadyToPackIncrease);
            totalPackingSpeedFactor = totalPackingSpeedFactor * (1 + ItemsBoughtList[i].BuyableItem.PackingSpeedIncrease);
        }

        MeatballsPerAnswer = BaselineMeatballsPerAnswer * totalPackableFactor;
        MeatballPackingPerSecond = BaselineMeatballPackingPerSecond * totalPackingSpeedFactor;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
