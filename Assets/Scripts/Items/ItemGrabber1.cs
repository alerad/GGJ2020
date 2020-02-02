using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGrabber1 : MonoBehaviour {

   public OVRHand hand;
   private bool handStaying;
   
   private GameObject itemInHand;
   private Transform itemToGrab;
   
   private void Update() {

      if (hand == null) {
         if (AnyPinching(hand) && handStaying)
            GrabItemLogic();
      }
      else {
         if (Input.GetKeyDown(KeyCode.G))
            GrabItemLogic();
      }
   }

   private bool AnyPinching(OVRHand x) =>
       x.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
             x.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
             x.GetFingerIsPinching(OVRHand.HandFinger.Pinky) ||
             x.GetFingerIsPinching(OVRHand.HandFinger.Ring);

   private void GrabItemLogic() {
      if (!ReferenceEquals(itemInHand, null))
         return;

      if (!ReferenceEquals(itemToGrab, null)) {
         itemToGrab.parent = transform;
      }
   }

   private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Ingredient")) {
         handStaying = true;
         itemToGrab = other.transform;
      }
   }

   private void OnTriggerExit(Collider other) {
      if (other.CompareTag("Ingredient")) {
         handStaying = false;
         itemToGrab = null;
      }
   }

}
