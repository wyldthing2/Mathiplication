using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressTracker : MonoBehaviour
{
    [SerializeField] Slider ProgressBar;
    [SerializeField] GameMaster gameMaster;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void SetProgressBar(float progressPercent)
    {
        ProgressBar.value = progressPercent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
