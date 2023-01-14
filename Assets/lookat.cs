using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookat : MonoBehaviour
{
    [SerializeField] Transform target;

    private void Start()
    {
        target = gameManager.instance.player.transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
