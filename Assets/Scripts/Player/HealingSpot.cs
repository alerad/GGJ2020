using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class HealingSpot : MonoBehaviour {
   public Problem.SpawnLocation p;
   private GameManager m;
   [SerializeField] private GameObject difficultyIndicator;
   
   private void Start() {
      difficultyIndicator = GetComponentInChildren<DifficultyIndicator>().gameObject;
      m = GameManager.Instance;
      var appropiateProblem = m.currentPatient.problems.Where(y => y.spawnLocation == p).DefaultIfEmpty(null).FirstOrDefault();
      if (appropiateProblem == null) {
         difficultyIndicator.SetActive(false);
      }
      else {
         difficultyIndicator.SetActive(true);
         difficultyIndicator.GetComponent<Renderer>().material.color = GetColorForDifficulty(m.currentPatient.difficulty);
      }
   }

   private Color GetColorForDifficulty(GameManager.Difficulty d) {
      switch (d) {
         case GameManager.Difficulty.Easy: return Color.green;
         case GameManager.Difficulty.Normal: return Color.yellow;
         case GameManager.Difficulty.Medium: return new Color(255, 105, 0);
         default: return Color.red;
      }
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
