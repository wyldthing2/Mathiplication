using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    TMP_Text buttonTextMesh;

    public void changeThisButtonText()
    {
        buttonTextMesh = this.GetComponentInChildren<TMP_Text>();
        buttonTextMesh.text = "Clicked";
    }
}
