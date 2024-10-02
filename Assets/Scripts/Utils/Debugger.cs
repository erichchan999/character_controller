using System;
using System.Transactions;
using UnityEngine;

namespace Utils
{
    public class Debugger : Debug {
        public static void logPosition(Transform t)
        {
            String s = String.Format("x: {0}\ty: {1}: z: {2}", t.position.x, t.position.y, t.position.z);
            Debug.Log(s);
        }
    }
}