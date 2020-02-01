    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    using UnityEngine;


    public class DoctorVision : MonoBehaviour {

     public Transform cam;
     public GameObject view;
     public OVRHand hand;
     private bool visionOn;


     private void Start()
     {
         visionOn = false;
     }

     void Update()
     {
        ButtonOffset();
        CheckDoctorVision();
    }

    void ButtonOffset()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft))
        {
            Vector3 pos = view.transform.localPosition;
            pos.x -= 0.01f;
            view.transform.localPosition = pos;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
        {
            Vector3 pos = view.transform.localPosition;
            pos.x += 0.01f;
            view.transform.localPosition = pos;
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp))
        {
            Vector3 pos = view.transform.localPosition;
            pos.y += 0.01f;
            view.transform.localPosition = pos;

        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown))
        {
            Vector3 pos = view.transform.localPosition;
            pos.y -= 0.01f;
            view.transform.localPosition = pos;
        }
    }

    private void CheckDoctorVision()
    {
        if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            EnableDoctorVision();
        }
        else
        {
            DisableDoctorVision();
        }
    }


    void EnableDoctorVision() {
        if (!visionOn)
        {
            view.SetActive(true);
            visionOn = true;
        }
    }

    void DisableDoctorVision() {
        if (visionOn)
        {
            view.SetActive(false);
            visionOn = false;
        }
    }

}
