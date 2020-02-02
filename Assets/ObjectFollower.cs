using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{

    public Transform target;
    public Vector3 offset = Vector3.zero;
    
   

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
        
    }
}
