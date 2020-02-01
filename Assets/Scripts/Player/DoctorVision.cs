using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoctorVision : MonoBehaviour
{

    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft)) {
            Vector3 pos = cam.transform.localPosition;
            pos.x -= 0.01f;
            cam.transform.localPosition = pos;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight)) {
            Vector3 pos = cam.transform.localPosition;
            pos.x += 0.01f;
            cam.transform.localPosition = pos;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp)) {
            Vector3 pos = cam.transform.localPosition;
            pos.y += 0.01f;
            cam.transform.localPosition = pos;

        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown)) {
            Vector3 pos = cam.transform.localPosition;
            pos.y -= 0.01f;
            cam.transform.localPosition = pos;
        }



    }
}
