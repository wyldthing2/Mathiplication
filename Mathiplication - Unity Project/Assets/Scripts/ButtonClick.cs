using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1);

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

        float objectWrapper = buttonTextMesh.GetComponent<RectTransform>().rect.height / 2;
    }

    

}
