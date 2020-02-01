using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnP : MonoBehaviour
{
    public GameObject patientslistph;
    public GameObject patient;
    GameObject lastpatient;
    public Vector3 patientspawnpoint;

    public float firstpatient;
    void Start()
    {   
        Invoke("SpawnPatient",firstpatient);
        
    }

   void Update()
   {
       if(Input.GetKeyDown(KeyCode.A)){
        DeletePatient();
       }
   }

    public void SpawnPatient(){
        if(lastpatient != null){
        lastpatient = Instantiate(patient,patientspawnpoint,Quaternion.identity);
        }



    }

    public void DeletePatient(){
       Object.Destroy(lastpatient);
        SpawnPatient();


    }
}
