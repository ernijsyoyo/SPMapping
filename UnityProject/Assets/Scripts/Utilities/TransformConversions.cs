using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SP
{

    [ExecuteInEditMode]
    public class TransformConversions : MonoBehaviour
    {
        // For testing..
        public GameObject pivot;
        public GameObject origin;
        public bool test;

        private void Update()
        {
            if (test)
            {
                test = false;
                print(posRelativeTo(pivot.transform.position, origin.transform.position));
                print(rotRelativeTo(pivot.transform.rotation, origin.transform.rotation).eulerAngles);
            }
        }

        /// <summary>
        /// Calculates an origin position that is relative to a pivot point
        /// </summary>
        /// <param name="pivot">Pivot point that origin is relative to</param>
        /// <param name="origin">Entity's origin that we wish to calculate a relative position to</param>
        /// <returns></returns>
        public static Vector3 posRelativeTo(Vector3 pivot, Vector3 origin)
        {
            return origin - pivot;
        }

        /// <summary>
        /// Calculates an origin rotation that is relative to a pivot point's rotation
        /// </summary>
        /// <param name="pivot">Pivot point that origin is relative to</param>
        /// <param name="origin">Entity's origin that we wish to calculate a relative rotation to</param>
        /// <returns></returns>
        public static Quaternion rotRelativeTo(Quaternion pivot, Quaternion origin)
        {
            return Quaternion.Inverse(pivot) * origin;
        }
    }
}
