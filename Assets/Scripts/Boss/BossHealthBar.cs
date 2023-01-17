using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    // NOTE: State index parameters we are inputting to these functions should be a 1, 2, or 3. No 0.

    [Header("--- Components ---")]
    [SerializeField] public Image healthBarFill;
    [SerializeField] public Image[] stateFills;
    [SerializeField] CanvasGroup cv;


    [Header("--- Lerping Only Adjust Speed and Refill/Destroy Delay---")]
    [SerializeField] float lerpSpeed;
    [SerializeField] int refillHealthDelay;
    [SerializeField] bool isLerpingHP;
    [SerializeField] bool isDestroyingHPBar;
    [SerializeField] bool isDraining;
    [SerializeField] bool isRefilling;
    //[SerializeField] int currentStateIndexLerping;
    [SerializeField] float currentLerpedVal;
    [SerializeField] float currentLerpToValue;


    private void Update()
    {
        isLerpingHealthBarDown();
        isLerpingHealthBarUp();
        isLerpingToDestroy();
    }

    private void isLerpingHealthBarDown()
    {
        if (isLerpingHP && isDraining && !isRefilling)
        {
            //Lerp HP
            currentLerpedVal = Mathf.Lerp(currentLerpedVal, currentLerpToValue, Time.deltaTime * lerpSpeed);
            healthBarFill.fillAmount = currentLerpedVal;
            //Check for if has reach end of lerp
            //If so, turn off isLerping and then turn off that image object
            if (currentLerpedVal <= currentLerpToValue + 0.01f)
            {
                isLerpingHP = false;
                isDraining = false;
            }
        }
    }

    private void isLerpingHealthBarUp()
    {
        if (isLerpingHP && isRefilling)
        {
            //Lerp HP
            currentLerpedVal = Mathf.Lerp(currentLerpedVal, 1, Time.deltaTime * lerpSpeed);
            healthBarFill.fillAmount = currentLerpedVal;

            //Check for if has reach end of lerp
            if (currentLerpedVal >= 0.99f)
            {
                isLerpingHP = false;
                isRefilling = false;
            }
        }
    }

    private void isLerpingToDestroy()
    {
        if (isLerpingHP && isDestroyingHPBar)
        {
            //Lerp all opacity to nothing
            currentLerpedVal = Mathf.Lerp(currentLerpedVal, 0, Time.deltaTime * lerpSpeed);
            cv.alpha = currentLerpedVal;

            //Check for if has reach end of lerp
            if (currentLerpedVal <= 0.01f)
            {
                isLerpingHP = false;
                isDestroyingHPBar = false;

                // Turn of this bosses health bar
                gameObject.SetActive(false);
            }
        }
    }

    // Public functions called from the bosses' scripts

    public void updateHealthFillAmount(float lerpFromValue, float lerpToValue, float maxHP)
    {
        beginLerpingHealthBar(lerpFromValue / maxHP, lerpToValue / maxHP);
    }

    public void turnOffState(int stateNum)
    {
        stateFills[stateNum - 1].gameObject.SetActive(false);
    }

    public IEnumerator refillHealthBar()
    {
        yield return new WaitForSeconds(refillHealthDelay);
        currentLerpedVal = 0;
        isLerpingHP = true;
        isRefilling = true;
    }

    public IEnumerator destroyHealthBar()
    {
        yield return new WaitForSeconds(refillHealthDelay);
        currentLerpedVal = 1;
        isLerpingHP = true;
        isDestroyingHPBar = true;
    }

    // Helpers

    private void beginLerpingHealthBar(float lerpFromValue, float lerpToValue)
    {
        // Store the new value to lerp FROM as the currentLerpedValue
        currentLerpedVal = lerpFromValue;

        // Store the new value to lerp to
        currentLerpToValue = lerpToValue;

        isLerpingHP = true;
        isDraining = true;
    }
}
