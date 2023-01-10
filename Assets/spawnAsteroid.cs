using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnAsteroid : MonoBehaviour
{
    [SerializeField] GameObject asteroid;
    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(instAsteroid());
    }
    IEnumerator instAsteroid()
    {

        GameObject newAsteroid = Instantiate(asteroid);
        ////set the transform of the new pipe
        //newAsteroid.transform.position = transform.position + new Vector3(-1, -1, 0);
        //newAsteroid.
            transform.position += Vector3.left * speed * Time.deltaTime;

        //Destroy the new pipe 15 seconds after loading the object

        yield return new WaitForSeconds(5);
    }
}
