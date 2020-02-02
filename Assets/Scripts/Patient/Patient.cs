using System;
using System.Collections.Generic;
using System.Linq;
using MonKey.Extensions;
using UnityEngine;

public class Patient : MonoBehaviour {
    public GameManager.Difficulty difficulty;
    public List<Problem> problems;
    public float patienceTime;

    private void Start() {
    }
    /// <summary>
    /// Tries to cure a player, based on a spawn location
    /// </summary>
    /// <param name="p"></param>
    /// <param name="spawnLocation"></param>
    public void TryCurePlayer(Potion p, Problem.SpawnLocation spawnLocation) {
        var potOk = IsPotionOkay(p, spawnLocation);
        if (potOk != null) {
            Debug.Log("Potion is ok");
            problems.Remove(potOk);
            if (problems.Count == 0)
                GameManager.Instance.OnPatientSucceed();
        }
        else {
            Debug.Log("Potion is wrong");
            GameManager.Instance.OnPatientFail();
        }
    }

    
    /// <summary>
    /// Checks if all ingredients in the potion match with any of the potions for the current problem
    /// </summary>
    /// <param name="p"></param>
    /// <returns>Which problem was healed, null if none</returns>
    private Problem IsPotionOkay(Potion p, Problem.SpawnLocation l) {
        if (p == null)
            return null;
        
        var passedProblem = problems.Where(x => {
            var potionOk = x
                .potions
                .DefaultIfEmpty(null)
                .FirstOrDefault(
                    c => c.ingredients.OrderBy(y => (int)y)
                        .SequenceEqual(p.ingredients
                            .OrderBy(av => (int)av)
                        )
                );
            return potionOk != null;
        })
            .DefaultIfEmpty(null)
            .FirstOrDefault();

        if (passedProblem != null && passedProblem.spawnLocation != l)
            return null; //TODO Aca ponerle feedback al usuario de que es el area incorrecta
        

        return passedProblem;
    }
    
    public void SetPatientData() {
        var difficulty = GameManager.Instance.currentDifficulty;
        var patienceTime = GetPatienceForDifficulty(difficulty);
        this.patienceTime = patienceTime;
        this.difficulty = difficulty;
        problems = GetProblems();
        problems.ForEach(x => {
            Debug.Log(x.name + " " + x.spawnLocation);
        });
    }

    private List<Problem> GetProblems() {
        List<Problem> problems = new List<Problem>();

        for (int i = 0; i < GetProblemsCountForDifficulty(difficulty); i++) {
            problems.Add(GetProblem(problems));
        }

        return problems;
    }

    private Problem GetProblem(List<Problem> problems) {
        var prob = GetRandomProblem(difficulty);
        if (problems.Count(x => x.spawnLocation == prob.spawnLocation) == 0)
            return prob;
        
        return GetProblem(problems);
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
            case GameManager.Difficulty.Easy: return 1;
            case GameManager.Difficulty.Normal: return 1;
            case GameManager.Difficulty.Medium: return 2;
            case GameManager.Difficulty.Hard: return 3;
        }

        return 2;
    }
    
}
