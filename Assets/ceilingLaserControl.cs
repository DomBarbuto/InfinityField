using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ceilingLaserControl : MonoBehaviour
{
    public void shutDownLaser()
    {
        gameObject.SetActive(false);
    }
}
