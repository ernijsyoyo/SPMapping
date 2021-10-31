using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SP
{
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
            

            //var tl = myLog.text.Length;
            //var sp = maxChars / 4;
            //if (tl > maxChars)
            //{
            //    myLog.text = myLog.text.Substring(sp, maxChars - sp);
            //}

            //myLog.text = logString;
            //string newString = "\n [" + type + "] : " + myLog;
            //myLogQueue.Enqueue(newString);
            //if (type == LogType.Exception)
            //{
            //    newString = "\n" + stackTrace;
            //    myLogQueue.Enqueue(newString);
            //}
            //myLog.text = string.Empty;
            //foreach (string mylog in myLogQueue)
            //{
            //    myLog.text += mylog;
            //}
            //print(myLog.text.Length);
        }
    }
}