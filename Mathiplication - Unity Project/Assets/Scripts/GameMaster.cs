using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    [SerializeField] SaveManager saveManager;

    [SerializeField] GameObject meatballSource;
    [SerializeField] LaunchMeatballs meatballLauncher;

    [SerializeField] TMP_Text FirstNumber;
    [SerializeField] TMP_Text SecondNumber;
    //[SerializeField] public List<AnswerNumber> AnswerList = new List<AnswerNumber>();
    static public GameMaster TheGameMaster;
    [SerializeField] int num1;
    [SerializeField] int num2;

    [SerializeField] public int MeatballCount = 0;
    [SerializeField] public int CurrentGoal = 100;
    [SerializeField] public float CurrentGoalProgress = 0;
    [SerializeField] float CurrentGoalWorkInProgress = 0;
    [SerializeField] TMP_Text meatballCountDisplay;
    [SerializeField] ProgressTracker myProgressTracker;
    [SerializeField] Slider myProgressTrackerWorkInProgress;
    [SerializeField] float progressBarSpeed = 1;


    [SerializeField] GameObject CorrectSplosion;
    [SerializeField] GameObject NotRightSplosion;


    // Start is called before the first frame update
    void Awake()
    {
        TheGameMaster = this;
        setNumbers();
        saveManager.Load();
        MeatballCount = saveManager.ThisSaveState.MeatballCount;
        meatballCountDisplay.text = MeatballCount.ToString();
        CurrentGoalProgress = (float)MeatballCount / (float)CurrentGoal;
        myProgressTracker.SetProgressBar(CurrentGoalProgress);
        CurrentGoalWorkInProgress = CurrentGoalProgress;
        myProgressTrackerWorkInProgress.value = CurrentGoalWorkInProgress;

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

    public void CheckAnswer(int answer)
    {
        Debug.Log(answer);
        if (answer == num1 * num2)
        {
            meatballLauncher.transform.position = meatballSource.transform.position;
            meatballLauncher.CommandLaunchXNumberOfMeatballsAtThisYTransform(num1 * num2, meatballLauncher.transform, myProgressTracker.ProgressBar.transform);
            if (CurrentGoalProgress < 1)
            {
                MeatballCount += answer;
                CurrentGoalProgress = (float)MeatballCount / (float)CurrentGoal;
                if(CurrentGoalProgress > 1 )
                {
                    CurrentGoalProgress = 1;
                }
                myProgressTracker.SetProgressBar(CurrentGoalProgress);
            }

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
        saveManager.ThisSaveState.MeatballCount = MeatballCount;
        //run save()
        saveManager.Save();
        meatballCountDisplay.text = MeatballCount.ToString("n0");
    }

    // Update is called once per frame
    void Update()
    {
        CurrentGoalWorkInProgress = Mathf.Lerp(CurrentGoalWorkInProgress, CurrentGoalProgress, Time.deltaTime);
        myProgressTrackerWorkInProgress.value = CurrentGoalWorkInProgress;
    }
}
