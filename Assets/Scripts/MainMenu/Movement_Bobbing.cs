using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Bobbing : MonoBehaviour
{
    Vector3 posAst;
    bool theSwitcher;
    // Start is called before the first frame update
    void Start()
    {
        posAst = new Vector3(0, 0.25f, 0) + transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 5 * Time.deltaTime, 0);
        //Want the ship to slowly move up and down
        StartCoroutine(Bobbing(theSwitcher));
        theSwitcher = !theSwitcher;
    }
    IEnumerator Bobbing(bool change)
    {
        if (change)
        {
            Vector3.Lerp(transform.position, posAst, 0.25f * Time.deltaTime);
        }
        else
        {
            Vector3.Lerp(posAst, transform.position, 0.25f * Time.deltaTime);
        }
        yield return new WaitForSeconds(2);
    }
}
