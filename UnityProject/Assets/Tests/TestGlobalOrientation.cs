using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGlobalOrientation : MonoBehaviour
{

    // Start is called before the first frame update
    void Start() {
        SP.GlobalOrigin.setPosition(Vector3.zero);
        SP.GlobalOrigin.setRot(new Quaternion(0, 0, 0, 0));
    }

    private void OrientationSetHandler(EventArgs args) {
        print("Position was set!");
    }
}
