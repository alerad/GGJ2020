using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Patient : MonoBehaviour {
    public GameManager.Difficulty difficulty;
    private List<Problem> problems;
    public float patienceTime;

    private void Start() {
        SetPatientData();
    }

    private bool IsPotionOkay(Potion p) {
        var potionOk = problems
            .First(x => x.potions.First(y => y == p));

        return potionOk == null;
    }
    
    private void SetPatientData() {
        var difficulty = GameManager.Instance.currentDifficulty;
        var patienceTime = GetPatienceForDifficulty(difficulty);
        this.patienceTime = patienceTime;
        this.difficulty = difficulty;
    }

    private float GetPatienceForDifficulty(GameManager.Difficulty d) {
        switch (d) {
            case GameManager.Difficulty.Easy: return 60f;
            case GameManager.Difficulty.Normal: return 70f;
            case GameManager.Difficulty.Medium: return 90f;
            case GameManager.Difficulty.Hard: return 100f;
        }

        return 100f;
    }

    //TODO Hacer que sea pseudorandom
//    private Problem GetRandomProblem() {
//        
//    }
}
