using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observations : MonoBehaviour
{
    [SerializeField] public GameObject[] modelsInOrder;
    [SerializeField] public int currentModel;

    private void Start()
    {
        int randomInitialModel = Random.Range(0, modelsInOrder.Length);
        currentModel = randomInitialModel;
        //turnOnModel(randomInitialModel);
    }

    public void turnOffModel(int index)
    {
        modelsInOrder[index].SetActive(false);
    }

    public void turnOnModel(int index)
    {
        modelsInOrder[index].SetActive(true);
    }

    public void buttonFunc_GoToNextModel()
    {
        turnOffModel(currentModel);
        currentModel++;
        if (currentModel >= modelsInOrder.Length)
            currentModel = 0;
        turnOnModel(currentModel);
    }

    public void buttonFunc_GoToPrevModel()
    {
        turnOffModel(currentModel);
        currentModel--;
        if (currentModel <= 0)
            currentModel = modelsInOrder.Length - 1;
        turnOnModel(currentModel);
    }


}
