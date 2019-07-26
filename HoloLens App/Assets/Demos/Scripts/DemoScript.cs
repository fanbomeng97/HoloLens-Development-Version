using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloRepository;
using TMPro;

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
        yield return StorageConnectionServer.GetAllPatient(patientList);
        //Do something here
        All.SetActive(true);
        foreach (PatientInfo patient in patientList)
        {
            GameObject button = Instantiate(buttonTemplates) as GameObject;
            button.SetActive(true);
            Debug.Log(patient.name.full);
            //button.GetComponent<DemoScript>().SetText(patient.name.full);

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
        //Do something here
        Single.SetActive(true);
        SinglePatientInfo.text = "Patient name: \n" + patient.name.full + "\nGender: " + patient.gender + "\nDate of birth: \n" + patient.birthDate.Substring(0, 10);
    }

    public void LoadModel()
    {
        ModelSetting.SetManipulable(true);
        ModelSetting.SetRotation(new Vector3(0, 180, 0));
        ModelSetting.SetPostition(new Vector3(0.2f, -0.18f, 0.8f));
        ModelSetting.SetSize(0.12f);
        StorageConnectionServer.LoadHologram("hololensid");
    }

    public void SetText(string name)
    {
        buttonText.text = name;
    }
}