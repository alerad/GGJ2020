using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSystemHelper
{
    public Quaternion GetRotation(Transform q) {
        Vector3 angle = q.eulerAngles;
        var rotation = q.rotation;
        float x = angle.x;
        float y = angle.y;
        float z = angle.z;

        Debug.Log($"ANGLE: {angle.y} y: {q.rotation.y}");

            
        return new Quaternion(0,0,0, q.rotation.w);
    }
}
