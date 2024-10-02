using System;
using UnityEngine;

namespace Utils {
    public class Utils {
        
        /* Deprecated
        public GameObject GetChildGameObject(MonoBehaviour parent, String childname, bool caseSensitive) {
            StringComparison StrCase = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            foreach (Transform c in parent.transform) {
                if (String.Equals(c.gameObject.name, childname, StrCase)) {
                    return c.gameObject;
                }
            }
            return null;
        }
        */
    }
}