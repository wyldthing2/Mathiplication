using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    float elapsedTime = 0;
    float velocity = 0;

    void Start()
    {
        //this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1);

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        this.gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, velocity);
        if (this.gameObject.GetComponent<RectTransform>().anchoredPosition.y < -167 && velocity > 0)
        {
            velocity = 0;
        }
    }

    TMP_Text buttonTextMesh;

    public void changeThisButtonText()
    {
        buttonTextMesh = this.GetComponentInChildren<TMP_Text>();
        buttonTextMesh.text = "Clicked";
        //velocity += 1;
        float objectWrapper = buttonTextMesh.GetComponent<RectTransform>().rect.height / 2;
    }

    

}
