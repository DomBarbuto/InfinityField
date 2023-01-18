using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkBouncing : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float bounceHeight = 0.3f;
    public float bounceSpeed = 1f;

    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        // Rotate the object
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Bounce the object
        float y = startY + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}