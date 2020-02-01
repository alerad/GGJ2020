using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DoctorVision : MonoBehaviour {
   public ReactiveProperty<bool> inDoctorVision;

   private void Start() {
      Observable.EveryUpdate().Subscribe(x => CheckDoctorVision())
   }

   private void CheckDoctorVision() {
      if (OVRHand.)
   }
}
