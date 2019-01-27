using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStat : MonoBehaviour {

    // UI Stuff
    private Image content;
    public float currFill;
    public float currVal;
    public float lerpSpeed;

    public float maxVal { get; set; }

    public float CurrVal
    {
        get
        {
            return currVal;
        }

        set
        {
            if (value > maxVal)
            {
                currVal = maxVal;
            }
            else if (value < 0)
            {
                currVal = 0;
            }
            else
            {
                currVal = value;
            }

            currFill = currVal / maxVal;
        }
    }



    // Use this for initialization
    void Start () {
        content = GetComponent<Image>();
        maxVal = 100;
    }
	
	// Update is called once per frame
	void Update () {
        if (currFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Init(float currValue, float maxValue)
    {
        maxVal = maxValue;
        currVal = currValue;
        currFill = CurrVal;
    }
}
