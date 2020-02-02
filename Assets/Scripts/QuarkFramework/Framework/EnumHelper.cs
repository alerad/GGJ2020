using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuarkFramework {
    public class EnumHelper
    {
       [Serializable]
       public enum Limb { LeftHand, RightHand, Head, RightFoot, LeftFoot }
       public enum Axis { X, Y, Z}
   
       public static float VecAxis(Axis axis, Vector3 vec) {
           switch (axis) {
               case Axis.X: return vec.x;
               case Axis.Y: return vec.y;
               case Axis.Z: return vec.z;
               default: return float.MinValue;
           }
       }
    }
}
