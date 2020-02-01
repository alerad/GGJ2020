using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class DoctorVision : MonoBehaviour {
   private OVRHand[] hands;
   
   public ReactiveProperty<bool> inDoctorVision;

   private void Start() {
      hands = new OVRHand[]
      {
         GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/OVRHandPrefab").GetComponent<OVRHand>(),
         GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/OVRHandPrefab").GetComponent<OVRHand>()
      };
      
      Observable.EveryUpdate().Subscribe(x => CheckDoctorVision());
   }

   private void CheckDoctorVision() {
      if (hands.All(x => x.GetFingerIsPinching(OVRHand.HandFinger.Index))
            && OnVisionTrigger()) {
                  
      }
   }

   private bool OnVisionTrigger() {
      throw new NotImplementedException();
   }
}
