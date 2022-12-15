using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkGlow : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] Material thisObjectsMaterial;
    public float currentBlinkRate;
    [SerializeField] Color currentEmissionColor;
    [SerializeField] float currentIntensityValue;

    private void Start()
    {
        
        currentEmissionColor = thisObjectsMaterial.GetColor("_EmissionColor");

    }

    private void Update()
    {
        currentIntensityValue = Mathf.PingPong(Time.time * currentBlinkRate, 2);
        if(isPlayer)
            currentEmissionColor.b = currentIntensityValue;
        else
            currentEmissionColor.r = currentIntensityValue;

        thisObjectsMaterial.SetColor("_EmissionColor", currentEmissionColor);
                   
    }
}
