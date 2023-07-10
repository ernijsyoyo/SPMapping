using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{
    /// <summary>
    /// Prints messages to a console inside the application during runtime, for debugging
    /// </summary>
    public class OnScreenLogger : MonoBehaviour
    {
        public Text myLog;
        private string output = "";
        Queue myLogQueue = new Queue();
        public int maxChars;

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            output = System.DateTime.Now + logString + "\n" + output;
            if(output.Length > maxChars)
            {
                var dog = output.Substring(0, output.Length - 250);
                output = dog;
            }
            myLog.text = output;
        }
    }
}