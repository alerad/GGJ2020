using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [Tooltip("How many patients for each difficulty scaling")]
    public int patientsForDifficultyChange;
    private int patientsSucceed;
    private int patientsFailed;
    private int difficultyInt =>
        Mathf.RoundToInt((patientsSucceed + patientsFailed) / patientsForDifficultyChange) / 4;
    
    private PatientDifficulty currentDifficulty =>
       difficultyInt
        > (int) PatientDifficulty.Hard
            ? PatientDifficulty.Hard
            : (PatientDifficulty) difficultyInt;

    private void Start() {
        PatientSpawner.Instance.SpawnPatient();
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

    public enum PatientDifficulty {
        Easy,
        Normal,
        Medium,
        Hard
    }
    
    
    
}
