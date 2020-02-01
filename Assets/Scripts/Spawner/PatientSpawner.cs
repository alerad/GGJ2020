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
        return currPatient;
    }
    
    public void DeletePatient(){
        Destroy(currPatient.gameObject);
    }

    public Patient GetPatient() => currPatient;

}
