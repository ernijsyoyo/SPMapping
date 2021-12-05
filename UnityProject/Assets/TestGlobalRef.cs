using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestGlobalRef : MonoBehaviour
{
    public bool test = false;

    // Update is called once per frame
    void Update()
    {
        if(test)
        {
            test = false;
            SP.GlobalOrigin.setPosition(gameObject.transform.position);
            SP.GlobalOrigin.setRot(gameObject.transform.rotation);

            print(SP.TransformConversions.posRelativeTo(SP.GlobalOrigin.getPosition(), gameObject.transform.position));
        }
    }
}
