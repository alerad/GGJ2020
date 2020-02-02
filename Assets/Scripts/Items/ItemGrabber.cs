using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGrabber : MonoBehaviour {
   
   private OVRHand[] m_hands;
   private bool[] isHandStaying;

   private GameObject itemInHand;
   private Transform itemToGrab;
   
   private void Start() {
      m_hands = new []
      {
         GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/OVRHandPrefab").GetComponent<OVRHand>(),
         GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/OVRHandPrefab").GetComponent<OVRHand>()
      };
      isHandStaying = new [] {false, false};
   }

   private void Update() {
      var pinchingHand = m_hands
         .Where(x =>
            x.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
            x.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
            x.GetFingerIsPinching(OVRHand.HandFinger.Pinky) ||
            x.GetFingerIsPinching(OVRHand.HandFinger.Ring) ||
            Input.GetKeyDown(KeyCode.G)
         )
         .DefaultIfEmpty(null)
         .First();
      
      
      GrabItemLogic(pinchingHand);
   }

   private void GrabItemLogic(OVRHand pinchingHand) {
      if (!ReferenceEquals(itemInHand, null))
         return;

      if (!ReferenceEquals(pinchingHand, null) && !ReferenceEquals(itemToGrab, null)) {
         itemToGrab.parent = transform;
      }
   }

   private void OnTriggerEnter(Collider other) {
      int handIdx = GetIndexFingerHandId(other);
      if (handIdx != -1) {
         isHandStaying[handIdx] = true;
         itemToGrab = other.transform;
      }
   }

   private void OnTriggerExit(Collider other) {
      int handIdx = GetIndexFingerHandId(other);

      if (handIdx != -1) {
         isHandStaying[handIdx] = false;
         itemToGrab = null;
      }
      
   }

   private int GetIndexFingerHandId(Collider collider)
   {
      //Checking Oculus code, it is possible to see that physics capsules gameobjects always end with _CapsuleCollider
      if (collider.gameObject.name.Contains("_CapsuleCollider"))
      {
         //get the name of the bone from the name of the gameobject, and convert it to an enum value
         string boneName = collider.gameObject.name.Substring(0, collider.gameObject.name.Length - 16);
         OVRPlugin.BoneId boneId = (OVRPlugin.BoneId)Enum.Parse(typeof(OVRPlugin.BoneId), boneName);
 
         //if it is the tip of the Index
         if (boneId == OVRPlugin.BoneId.Hand_Index3)
            //check if it is left or right hand, and change color accordingly.
            //Notice that absurdly, we don't have a way to detect the type of the hand
            //so we have to use the hierarchy to detect current hand
            if (collider.transform.IsChildOf(m_hands[0].transform))
            {
               return 0;
            }
            else if (collider.transform.IsChildOf(m_hands[1].transform))
            {
               return 1;
            }
      }
 
      return -1;
   }
}
