using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    /// <summary>
    /// Retrieve and set a global origin point of the system
    /// </summary>
    public static class GlobalOrigin
    {
        private static Transform globalTransform;
        private static Quaternion rotation;
        private static bool posSet = false;
        private static bool rotSet = false;
        private static int counter = 0;

        public delegate void OrientationSet(EventArgs args);
        /// <summary>
        /// Event that is raised once position and rotation is set
        /// </summary>
        public static event OrientationSet OnOrientationSet;

        /// <summary>
        /// Get the global origin's location
        /// </summary>
        /// <returns></returns>
        public static Transform getTransform() { return globalTransform; }

        /// <summary>
        /// Get the global origin's rotation
        /// </summary>
        /// <returns></returns>
        public static Quaternion getRot() { return rotation; }

        /// <summary>
        /// Set the global point of origin. Can be set only once 
        /// </summary>
        /// <param name="newPos"></param>
        /// <returns>True if a position is set</returns>
        public static bool setTransform(Transform newTransform)
        {
            if (posSet == false) {
                if (counter > 25) // allows the marker to settle down..
                {
                    Debug.Log("rotation set");
                    posSet = true;
                    attemptRaiseEvent();
                }
                counter += 1;
                Debug.Log("Setting global reference point.. " + counter);
                globalTransform = newTransform;
            }
            return posSet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newRot"></param>
        /// <returns>True if a rotation is set</returns>
        public static bool setRot(Quaternion newRot)
        {
            if (!rotSet)
            {
                if (counter > 25) // allows the marker tracking to settle down..
                {
                    Debug.Log("rotation set");
                    rotSet = true;
                    attemptRaiseEvent();
                }
                rotation = newRot;
            }
            return rotSet;
        }

        /// <summary>
        /// Attempt to raise the onOrientationSet event if rot and pos is set
        /// </summary>
        private static void attemptRaiseEvent()
        {
            if (rotSet && posSet)
            {
                OnOrientationSet?.Invoke(EventArgs.Empty);
            }
        }

        public static void resetPosRot()
        {
            rotation = new Quaternion(0, 0, 0, 0);
            posSet = false;
            rotSet = false;
        }
    }
}
