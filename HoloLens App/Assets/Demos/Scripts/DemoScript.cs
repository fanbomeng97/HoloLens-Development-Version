using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloRepository;
using TMPro;
using System;

public class DemoScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI SinglePatientInfo = null;
    [SerializeField]
    private TextMeshProUGUI buttonText = null;
    [SerializeField]
    private GameObject buttonTemplates = null;
    [SerializeField]
    private GameObject All = null;
    [SerializeField]
    private GameObject Single = null;

    public void GetAllPAteint()
    {
        StartCoroutine(getAllPateint());
    }

    IEnumerator getAllPateint()
    {
        List<PatientInfo> patientList = new List<PatientInfo>();
        yield return StorageConnectionServer.GetMultiplePatient(patientList,"IDs");
        All.SetActive(true);
        foreach (PatientInfo patient in patientList)
        {
            GameObject button = Instantiate(buttonTemplates) as GameObject;
            button.SetActive(true);
            button.GetComponent<DemoScript>().SetText(patient.name.full);
            button.transform.SetParent(buttonTemplates.transform.parent, false);
        }
    }

    public void GetPatientByID()
    {
        StartCoroutine(getPateintByID());
    }

    IEnumerator getPateintByID()
    {
        PatientInfo patient = new PatientInfo();
        yield return StorageConnectionServer.GetPatient(patient, "5d1bf4f17322a9283482fe7e");
        Single.SetActive(true);
        try
        {
            SinglePatientInfo.text = string.Format("Patient name: \n{0}\nGender: {1}\nDate of Birth: \n{2}", patient.name.full, patient.gender, patient.birthDate.Substring(0, 10));
        }
        catch (Exception e)
        {
            Debug.Log("Failed to set the text: " + e.Message);
        }    
    }

    public void LoadModel()
    {
        ModelSetting.SetManipulable(true);
        ModelSetting.SetRotation(new Vector3(0, 180, 0));
        ModelSetting.SetPostition(new Vector3(0.22f, -0.2f, 0.8f));
        ModelSetting.SetSize(0.12f);
        StorageConnectionServer.LoadHologram("hololensid");
    }

    public void SetText(string name)
    {
        buttonText.text = name;
    }
}