using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{

    [SerializeField]
    private float lerpSpeed;
    private Image content;
    private float currentFill;
    private float currentValue;
    public float myMaxValue { get; set; }

	public float myCurrentValue
    {
        get
        {
            return currentValue;
        }

        set
        {
            if(value > myMaxValue)
                currentValue = myMaxValue;
            else if(value < 0)
                currentValue = 0;
            else
                currentValue = value;

            currentFill = currentValue / myMaxValue;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float currentValue, float maxValue)
	{
        myMaxValue = maxValue;
        myCurrentValue = currentValue;
	}

    public bool TakeDamage(float amount)
	{
        myCurrentValue -= amount;
        return myCurrentValue == 0;
	}

    public void Heal(float amount)
    {
        myCurrentValue += amount;
    }
}
