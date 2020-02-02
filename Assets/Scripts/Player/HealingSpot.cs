using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingSpot : MonoBehaviour {
   public Problem.SpawnLocation p;
   private GameManager m;
   
   private void Start() {
      m = GameManager.Instance;
   }

   private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("PlayerHand") && m.potionInHand != null) {
         if (m.currentPatient.problems[0].spawnLocation != p)
            return; //TODO Feedback
         Debug.Log("Trying to heal!");
         m.currentPatient.TryCurePlayer(m.potionInHand, p);
      }
   }
}
