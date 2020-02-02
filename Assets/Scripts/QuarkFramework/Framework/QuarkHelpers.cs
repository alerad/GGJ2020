using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuarkFramework.EnumHelper;

namespace QuarkFramework {
    public static class VectorExtensions {
        public static float GetAxis(this Vector3 local, Axis axis)
        {
            switch (axis) {
                case Axis.X: return local.x;
                case Axis.Y: return local.y;
                case Axis.Z: return local.z;
                default: return float.MinValue;
            }
        }
       
        public static List<VectorAxis> AsList(this Vector3 local) => new List<VectorAxis> {
            new VectorAxis(local.x, Axis.X),
            new VectorAxis(local.y, Axis.Y),
            new VectorAxis(local.z, Axis.Z)
        };
        
        public static Vector3 IncrementByAxis(this Vector3 local, Axis axis, float number) {
            switch (axis) {
                case Axis.X: return new Vector3(local.x+number, 0, 0);
                case Axis.Y: return new Vector3(0, local.y+number, 0);
                case Axis.Z: return new Vector3(0, 0, local.z+number);
                default: return Vector3.zero;
            }
        }
        
//        public static void Add(this Vector3 local, Axis axis, float number) {
//            var newVec = local.IncrementByAxis(axis, number);
//            local = newVec;
//        }
        
        public static float GetAxis(this QuarkQuaternion local, Axis axis)
        {
            switch (axis) {
                case Axis.X: return local.x;
                case Axis.Y: return local.y;
                case Axis.Z: return local.z;
                default: return float.MinValue;
            }
        }
        
        
    }
    
    
}
