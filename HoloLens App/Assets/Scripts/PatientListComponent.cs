using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatientListComponent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Patientinfo;
    [SerializeField]
    private Text PatientID;

    private void Start()
    {
        Patientinfo = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string Patientinformation)
    {
        Patientinfo.text = Patientinformation;
    }
    public void SetID(string id)
    {
        PatientID.text = id;
    }
}
