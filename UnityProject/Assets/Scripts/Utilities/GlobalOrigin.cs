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
        private static Vector3 position;
        private static Quaternion rotation;
        private static bool posSet = false;
        private static bool rotSet = false;

        /// <summary>
        /// Get the global origin's location
        /// </summary>
        /// <returns></returns>
        public static Vector3 getPosition() { return position; }

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
        public static bool setPosition(Vector3 newPos)
        {
            if (posSet == false)
            {
                position = newPos;
                return true;
            }
            else
                return false;
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
                rotation = newRot;
                return true;
            }
            else
                return false;
        }

        public static void resetPosRot()
        {
            position = Vector3.zero;
            rotation = new Quaternion(0, 0, 0, 0);
            posSet = false;
            rotSet = false;
        }
    }
}
