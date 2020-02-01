using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSpawner : Singleton<PatientSpawner>
{
    [SerializeField]
    private Patient patient;
    private Patient currPatient;
    public Transform patientspawnpoint;
    
   public void SpawnAndDelete() {
       DeletePatient();
       SpawnPatient();
   }
   
   public void SpawnFirstPatient() {
       var p = SpawnPatient();
       p.patienceTime = Mathf.Infinity;
   }

   
    public Patient SpawnPatient() {
        currPatient = Instantiate(patient);
        currPatient.transform.position = patientspawnpoint.position;
        SetPatientData(currPatient);
        return currPatient;
    }
    
    public void DeletePatient(){
        Destroy(currPatient.gameObject);
    }

    private void SetPatientData(Patient p) {
        var difficulty = GameManager.Instance.currentDifficulty;
        var patienceTime = GetPatienceForDifficulty(difficulty);
        p.patienceTime = patienceTime;
        p.difficulty = difficulty;
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
}
