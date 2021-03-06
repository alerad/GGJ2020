﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGrabber1 : MonoBehaviour {

   public OVRHand hand;
   private bool handStaying;

   private Cauldron cauldron;
   
   private GameObject itemInHand;
   private Transform itemToGrab;
   
   private void Update() {

      if (hand != null) {
         if (!hand.IsTracked && itemInHand != null) {
            itemInHand.SetActive(false);
            return;
         }
         else {
            //TODO Si deja de seguirlo, probar re-poner el target
            if (itemInHand != null)
               itemInHand.SetActive(true);
         }
         
         if (AnyPinching(hand) && handStaying)
            GrabItemLogic();
         else if (cauldron != null && !AnyPinching(hand))
            AddCauldronIngredient();
      }
      else {
         if (Input.GetKeyDown(KeyCode.G))
            GrabItemLogic();

         if (Input.GetKeyDown(KeyCode.E) && cauldron != null)
            AddCauldronIngredient();

      }
   }

   private void AddCauldronIngredient() {
      Debug.Log("Dropping into cauldron");
      cauldron.AddIngredient(IngredientMixer.GetIngredientByName(itemInHand.name));
     

      Destroy(itemInHand);
      itemToGrab = null;
      itemInHand = null;

   }

   private bool AnyPinching(OVRHand x) =>
       x.GetFingerIsPinching(OVRHand.HandFinger.Index) ||
             x.GetFingerIsPinching(OVRHand.HandFinger.Middle) ||
             x.GetFingerIsPinching(OVRHand.HandFinger.Pinky) ||
             x.GetFingerIsPinching(OVRHand.HandFinger.Ring);

   private void GrabItemLogic() {
      Debug.Log("Grabbint item logic");
      if (!ReferenceEquals(itemInHand, null))
         return;

      if (!ReferenceEquals(itemToGrab, null)) {
         var of = itemToGrab.GetComponent<ObjectFollower>();
         of.target = transform;
         itemInHand = itemToGrab.gameObject;
      }
   }

   private void OnTriggerEnter(Collider other) {
      if (!isTracked)
         return;
      
      if (other.CompareTag("Ingredient")) {
         Debug.Log("Trigger enter ingredient");
         handStaying = true;
         itemToGrab = other.transform;
      }

      if (other.CompareTag("Cauldron")) {
         Debug.Log("Enter the cauldron");
         cauldron = other.gameObject.GetComponentInParent<Cauldron>();
      }
   }

   private void OnTriggerExit(Collider other) { 
      if (!isTracked)
         return;
   
      if (other.CompareTag("Ingredient")) {
         Debug.Log("Trigger exit ingredient");
         handStaying = false;
         itemToGrab = null;
      }

      if (other.CompareTag("Cauldron")) {
         Debug.Log("Exit the cauldron");
         cauldron = null;
      }
   }

   private bool isTracked => hand == null || hand.IsTracked;

}
