using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Singleton class
public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake() // happens before anything else, anytime object is turned on or off while start will only be called once
    {
        instance = this; //assign current object as the singleton instance
    }

    public TMP_Text overheatMessage;
    public Slider heatGauge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
