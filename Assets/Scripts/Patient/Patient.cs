﻿using System;
using System.Collections.Generic;
using System.Linq;
using MonKey.Extensions;
using UnityEngine;

public class Patient : MonoBehaviour {
    public GameManager.Difficulty difficulty;
    public List<Problem> problems;
    private int currProblem = 0;
    public float patienceTime;

    private void Start() {
        SetPatientData();
    }

    
    public void TryCurePlayer(Potion p) {
        if (IsPotionOkay(p)) {
            Debug.Log("Potion is ok");
            if (problems.Count - 1 == currProblem)
                GameManager.Instance.OnPatientSucceed();
        }
        else {
            Debug.Log("Potion is wrong");
            GameManager.Instance.OnPatientFail();
        }

        currProblem++;
    }

    
    /// <summary>
    /// Checks if all ingredients in the potion match with any of the potions for the current problem
    /// </summary>
    /// <param name="p"></param>
    /// <returns>True if potion cures patient</returns>
    private bool IsPotionOkay(Potion p) {
        if (p == null)
            return false;
        
        var potionOk = problems[currProblem]
            .potions
            .DefaultIfEmpty(null)
            .FirstOrDefault(
                x => x.ingredients.OrderBy(y => (int)y)
                    .SequenceEqual(p.ingredients
                        .OrderBy(av => (int)av)
                    )
            );

        return potionOk != null;
    }
    
    private void SetPatientData() {
        var difficulty = GameManager.Instance.currentDifficulty;
        var patienceTime = GetPatienceForDifficulty(difficulty);
        this.patienceTime = patienceTime;
        this.difficulty = difficulty;
        problems = GetProblems();
    }

    private List<Problem> GetProblems() {
        List<Problem> problems = new List<Problem>();
        
        for (int i = 0; i < GetProblemsCountForDifficulty(difficulty); i++)
            problems.Add(GetRandomProblem(difficulty));

        return problems;
    }
    
    private Problem GetRandomProblem(GameManager.Difficulty d) =>
        ScriptableObjectContainer.Instance.problems.Where(x => x.difficulty <= d).ToList().GetRandom();

    private float GetPatienceForDifficulty(GameManager.Difficulty d) {
        switch (d) {
            case GameManager.Difficulty.Easy: return 60f;
            case GameManager.Difficulty.Normal: return 70f;
            case GameManager.Difficulty.Medium: return 90f;
            case GameManager.Difficulty.Hard: return 100f;
        }

        return 100f;
    }
    
    private int GetProblemsCountForDifficulty(GameManager.Difficulty d) {
        switch (d) {
            case GameManager.Difficulty.Easy: return 2;
            case GameManager.Difficulty.Normal: return 1;
            case GameManager.Difficulty.Medium: return 1;
            case GameManager.Difficulty.Hard: return 2;
        }

        return 2;
    }
    
}
