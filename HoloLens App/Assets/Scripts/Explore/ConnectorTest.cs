using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTest : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        List<PatientInfo> patientList = StorageConnectionServer.GetAllPatient();
        foreach (PatientInfo patient in patientList)
        {
            Debug.Log(patient.pid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
