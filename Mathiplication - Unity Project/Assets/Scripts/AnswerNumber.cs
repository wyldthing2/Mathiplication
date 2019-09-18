using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerNumber : MonoBehaviour
{
    [SerializeField] int answerNumber;

    public void submitAnswer()
    {
        GameMaster.TheGameMaster.CheckAnswer(answerNumber);
    }

}
