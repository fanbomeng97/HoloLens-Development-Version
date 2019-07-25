using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {        
        StartCoroutine(getpatient());        
    }

    IEnumerator getpatient()
    {
        List<employee> patientList = new List<employee>();
        yield return StorageConnectionServer.GetAllPatient(patientList);
        //Do something
        foreach (employee patient in patientList)
        {
            Debug.Log(patient.id + patient.employee_name);
        }
    }
}
