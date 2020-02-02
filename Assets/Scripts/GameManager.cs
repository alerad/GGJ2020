using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [Tooltip("How many patients for each difficulty scaling")]
    public int patientsForDifficultyChange;
    private int patientsSucceed;
    private int patientsFailed;
    
    public Potion potionInHand;
    
    private int difficultyInt =>
        Mathf.RoundToInt((patientsSucceed + patientsFailed) / patientsForDifficultyChange);

    public Patient currentPatient => PatientSpawner.Instance.GetPatient();
    
    public Difficulty currentDifficulty =>
       difficultyInt
        > (int) Difficulty.Hard
            ? Difficulty.Hard
            : (Difficulty) difficultyInt;

    private void Start() {
        PatientSpawner.Instance.SpawnFirstPatient();
       
        Observable.EveryUpdate()
            .Where(x => Input.GetKeyDown(KeyCode.A))
            .Subscribe(x => {
                var ingsFromProblem = currentPatient.problems.First().potions.SelectMany(b => b.ingredients);
                var p = IngredientMixer.MixPotion(ingsFromProblem.ToList());
                currentPatient.TryCurePlayer(p, Problem.SpawnLocation.Intestines);
            });
        
    }

    //Todo Validacion de que no se llame 2 veces muy rapido
    public void OnPatientSucceed() {
        Debug.Log("Patient succeded");
        patientsSucceed++;
        potionInHand = null;
        PatientSpawner.Instance.SpawnAndDelete();
    }

    public void OnPatientFail() {
        Debug.Log("Patient failed");
        patientsFailed++;
        PatientSpawner.Instance.SpawnAndDelete();
    }

    public enum Difficulty {
        Easy,
        Normal,
        Medium,
        Hard
    }
    
    
}
