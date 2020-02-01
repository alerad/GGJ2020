using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [Tooltip("How many patients for each difficulty scaling")]
    public int patientsForDifficultyChange;
    private int patientsSucceed;
    private int patientsFailed;
    private int difficultyInt =>
        Mathf.RoundToInt((patientsSucceed + patientsFailed) / patientsForDifficultyChange);
    
    public Difficulty currentDifficulty =>
       difficultyInt
        > (int) Difficulty.Hard
            ? Difficulty.Hard
            : (Difficulty) difficultyInt;

    private void Start() {
        PatientSpawner.Instance.SpawnFirstPatient();
        Observable.EveryUpdate().Where(x => Input.GetKeyDown(KeyCode.A)).Subscribe(x => OnPatientFail());
    }

    //Todo Validacion de que no se llame 2 veces muy rapido
    public void OnPatientSucceed() {
        patientsSucceed++;
        PatientSpawner.Instance.SpawnAndDelete();
    }

    public void OnPatientFail() {
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
