using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    // NOTE: State index parameters we are inputting to these functions should be a 1, 2, or 3. No 0.

    [Header("--- Components ---")]
    [SerializeField] public Image healthbarFill;
    [SerializeField] public Image[] stateFills;

    [Header("--- General ---")]
    [SerializeField] float lerpSpeed;
    [SerializeField] bool isLerpingHP;
    [SerializeField] bool isLerpingStateFills;

    [Header("--- Lerping ---")]
    [SerializeField] int currentStateIndexLerping;
    [SerializeField] float currentLerpedVal;


    private void Update()
    {
        isLerpingHealthBar();
        LerpStateFills();
    }

    private void isLerpingHealthBar()
    {
        if (isLerpingHP)
        {
            //Lerp HP
            healthbarFill.fillAmount = Mathf.Lerp(currentLerpedVal, 0, Time.deltaTime * lerpSpeed);

            //Check for if has reach end of lerp
            //If so, turn off isLerping and then turn off that image object
            if (currentLerpedVal <= 0)
            {
                isLerpingHP = false;
                healthbarFill.gameObject.SetActive(false);
            }
        }
    }

    private void LerpStateFills()
    {
        if (isLerpingStateFills)
        {
            //Lerp
            stateFills[currentStateIndexLerping].fillAmount = Mathf.Lerp(currentLerpedVal, 0, Time.deltaTime * lerpSpeed);

            //Check for if has reach end of lerp
            //If so, turn off isLerping and then turn off that image object
            if (currentLerpedVal <= 0)
            {
                isLerpingStateFills = false;
                stateFills[currentStateIndexLerping].gameObject.SetActive(false);
            }
        }
    }

    // Public functions called from the bosses' scripts

    public void updateHealthFillAmount(float lerpFromValue, float lerpToValue)
    {
        beginLerpingHealthBar(lerpFromValue, lerpToValue);
    }

    public void turnOffState(int stateNum)
    {
        beginLerpingStateFill(stateNum);
    }

    // Helpers

    private void beginLerpingStateFill(int stateFillIndex)
    {
        currentStateIndexLerping = stateFillIndex - 1;

        // Store the new value to lerp FROM as the currentLerpedValue
        currentLerpedVal = stateFills[currentStateIndexLerping].fillAmount;
    }

    private void beginLerpingHealthBar(float lerpFromValue, float lerpToValue)
    {
        // Store the new value to lerp FROM as the currentLerpedValue
        currentLerpedVal = lerpFromValue;
        isLerpingHP = true;
    }
}
