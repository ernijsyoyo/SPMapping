using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    public bool test = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(test)
        {
            test = false;
            print("Hello world Hello world Hello world Hello world Hello world");
        }
    }
}
