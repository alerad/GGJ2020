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
      if (other.CompareTag("PlayerHand")) 
         m.currentPatient.TryCurePlayer(m.potionInHand, p);
   }
}
