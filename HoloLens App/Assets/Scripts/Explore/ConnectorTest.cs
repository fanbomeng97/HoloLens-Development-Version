using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloRepository;

public class ConnectorTest : MonoBehaviour
{
    void Start()
    {        
        StartCoroutine(QueryMethod());
        LoadModel();
    }

    IEnumerator QueryMethod()
    {        
        List<PatientInfo> patientList = new List<PatientInfo>();
        //Get all patients
        yield return StorageConnectionServer.GetAllPatient(patientList);
        foreach (PatientInfo patient in patientList)
        {
            Debug.Log("From List: "+ patient.name.full);
        }


        PatientInfo Patient = new PatientInfo();
        //Get patients with ID  
        yield return StorageConnectionServer.GetPatient(Patient, "5d1bf4f17322a9283482fe7e");
        Debug.Log("=========================================");
        Debug.Log("From single patient: " + Patient.name.full);
    }

    void LoadModel()
    {
        ModelSetting.SetManipulable(true);
        ModelSetting.SetRotation(new Vector3(0, 180, 0));
        ModelSetting.SetPostition(new Vector3(0, 0, 2f));
        ModelSetting.SetSize(1f);
        StorageConnectionServer.LoadHologram("hololensid");
    }
}
