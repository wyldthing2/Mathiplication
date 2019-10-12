using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMaster : MonoBehaviour
{
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
    [SerializeField] ProgressTracker myProgressTracker;

    [SerializeField] GameObject CorrectSplosion;
    [SerializeField] GameObject NotRightSplosion;


    // Start is called before the first frame update
    void Start()
    {
        TheGameMaster = this;
        setNumbers();

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
