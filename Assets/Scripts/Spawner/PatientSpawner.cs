using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientSpawner : Singleton<PatientSpawner>
{
    [SerializeField]
    private GameObject patient;
    private GameObject lastpatient;
    public Transform patientspawnpoint;
    
   void Update() {
       if (Input.GetKeyDown(KeyCode.A))
            DeletePatient();
   }

   public void SpawnAndDelete() {
       DeletePatient();
       SpawnPatient();
   }
   
    public void SpawnPatient(){
        if (lastpatient != null)
            lastpatient = Instantiate(patient);
        lastpatient.transform.position = patientspawnpoint.position;
    }

    public void DeletePatient(){
        Destroy(lastpatient);
        SpawnPatient();
    }
}
