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
        yield return StorageConnectionServer.GetMultiplePatient(patientList, "IDs");

        foreach (PatientInfo patient in patientList)
        {
            Debug.Log("From List: "+ patient.name.full);
        }


        PatientInfo Patient = new PatientInfo();
        //Get patients with ID  
        yield return StorageConnectionServer.GetPatient(Patient, "666da72f-1dfa-427a-96a9-c9fb30bf7296");

        Debug.Log("=========================================");
        Debug.Log("From single patient: " + Patient.name.full);


        List<HoloGrams> hologramList = new List<HoloGrams>();
        //Get all patients
        yield return StorageConnectionServer.GetMultipleHologram(hologramList, "IDs");
        Debug.Log("=========================================");
        foreach (HoloGrams hologram in hologramList)
        {
            Debug.Log("From List: " + hologram.subject.pid);
        }
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
