using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonClick : MonoBehaviour {

    public Button connectButton;
    public Text connectionText;

    private void Start()
    {
        connectButton.onClick.AddListener(TaskOnClick);
    }
    
    void TaskOnClick()
    {
        connectionText.text = "Connected";
        connectionText.color = Color.green;
    }
}
