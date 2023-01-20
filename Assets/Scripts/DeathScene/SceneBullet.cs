using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBullet : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] int DestroyTime; 
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = (transform.forward) * Speed;
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
