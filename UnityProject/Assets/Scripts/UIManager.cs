using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UIElements; //Need this for calling UI scripts
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{

    private MLInput.Controller controller;
    public GameObject HeadlockedCanvas;
    public GameObject controllerInput;
    public UnityEvent OnHomeButtonTap;

    // Start is called before the first frame update


    [System.Obsolete]
    void Start()
    {
        MLInput.Start();
        controller = MLInput.GetController(MLInput.Hand.Left);

        
    }

    void Update()
    {
        if (controller.TriggerValue > 0.5f)
        {
            RaycastHit hit;
            if(Physics.Raycast(controllerInput.transform.position, controllerInput.transform.forward, out hit))
            {
                if (hit.transform.gameObject.name == "StartButton")
                {
                    StartApp();
                }
            }
        }
    }

    // Update is called once per frame
    void StartApp()
    {
        HeadlockedCanvas.SetActive(false); 

    }

    [System.Obsolete]
    private void OnDestroy ()
    {
        MLInput.Stop();
    }

    void Export()
    {
        OnHomeButtonTap?.Invoke();
    }
}
