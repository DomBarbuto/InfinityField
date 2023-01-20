using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observations : MonoBehaviour
{
    [SerializeField] public GameObject[] modelsInOrder;
    [SerializeField] public GameObject[] textInOrder;
    [SerializeField] public int currentModel;
    [SerializeField] public int currentText;

    private void Start()
    {
        int randomInitialModel = Random.Range(0, modelsInOrder.Length);
        currentModel = randomInitialModel;
        currentText = randomInitialModel;
        //turnOnModel(randomInitialModel);
    }

    public void turnOffModel(int index)
    {
        modelsInOrder[index].SetActive(false);
        textInOrder[index].SetActive(false);
    }

    public void turnOnModel(int index)
    {
        modelsInOrder[index].SetActive(true);
        textInOrder[index].SetActive(true);
    }

    public void buttonFunc_GoToNextModel()
    {
        turnOffModel(currentModel);
        currentModel++;
        currentText++;
        if (currentModel >= modelsInOrder.Length)
        {
            currentModel = 0;
            currentText = 0;
        }
        turnOnModel(currentModel);
    }

    public void buttonFunc_GoToPrevModel()
    {
        turnOffModel(currentModel);
        currentModel--;
        currentText--;
        if (currentModel <= 0)
        {
            currentModel = modelsInOrder.Length - 1;
            currentText = textInOrder.Length - 1;
        }
        turnOnModel(currentModel);
    }


}
