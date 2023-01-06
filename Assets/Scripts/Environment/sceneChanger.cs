using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChanger : MonoBehaviour
{
    [SerializeField] int scene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
        }
    }
}
