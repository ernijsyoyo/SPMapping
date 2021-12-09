using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ButtonHandler : MonoBehaviour
{
    private MLInput.Controller _controller;
    public SP.NetworkManagerTCP tcpNetwork;
    public SP.SceneExport sceneExport;
    // Start is called before the first frame update
    void Start()
    {
        MLInput.OnControllerButtonDown += OnButtonDown;
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }

    private void OnButtonDown(byte controllerId, MLInput.Controller.Button button)
    {
        print("Button pressed");
        if (button == MLInput.Controller.Button.Bumper)
        {
            print("Bumper pressed");
            tcpNetwork.ConnectToTcpServer();
            sceneExport.ExportScene();
        }

    }
}
