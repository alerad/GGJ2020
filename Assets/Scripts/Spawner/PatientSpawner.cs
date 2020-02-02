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

    private void Start()
    {
        if (patientspawnpoint == null)
            throw new Exception("Patient spawn point not set");
    }

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
        RandomColorForPlayer();
        currPatient.SetPatientData();
        return currPatient;
    }

    void RandomColorForPlayer()
    {
        Renderer r = currPatient.GetComponent<Renderer>();
        Color c = r.material.color;
        r.material.color = RandomHelper.RandomColor(c);
    }
    
    public void DeletePatient(){
        Destroy(currPatient.gameObject);
    }

    public Patient GetPatient() => currPatient;

}
