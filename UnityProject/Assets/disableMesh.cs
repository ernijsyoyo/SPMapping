using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableMesh : MonoBehaviour
{
    public GameObject mesh;

    public void ToggleMesh()
    {
        print("Toggling gameobject: " + mesh.gameObject.name + " to state" + !mesh.activeSelf);
        mesh.SetActive(!mesh.activeSelf);
    }
}
