using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;
using HoloRepository;

public class GeneratePatientList : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplates = null;

    public static List<PatientInfo> patientList = new List<PatientInfo>();
    private List<employee> employeeList = new List<employee>();

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log("Web Error: " + webRequest.error);
            }
            else
            {
                //Debug.Log(pages[page] + ":Received: " + webRequest.downloadHandler.text);
                string json = webRequest.downloadHandler.text;

                patientList.Clear();
                JsonData jsonData = JsonMapper.ToObject(json);
                for (int i = 0; i < jsonData.Count; i++)
                {
                    PatientInfo patient = JsonMapper.ToObject<PatientInfo>(jsonData[i].ToJson());
                    patientList.Add(patient);
                }
                foreach (PatientInfo patient in patientList)
                {
                    GameObject button = Instantiate(buttonTemplates) as GameObject;
                    button.SetActive(true);

                    button.GetComponent<PatientListComponent>().SetID(patient.pid);
                    button.GetComponent<PatientListComponent>().SetText("Patient name: " + patient.name.full + "\nGender: " + patient.gender + "\nDate of birth: " + patient.birthDate.Substring(0, 10));
                    button.GetComponent<PatientListComponent>().SetImage(patient.gender);

                    button.transform.SetParent(buttonTemplates.transform.parent, false);
                }
            }
            
        }
    }

    /*public void ReadJsonFile()
    {
        StreamReader reader = new StreamReader("./Assets/Sample/samplePatientsWithHolograms.json");
        string json = reader.ReadToEnd();

        patientList.Clear();
        JsonData jsonData = JsonMapper.ToObject(json);
        for (int i = 0; i < jsonData.Count; i++)
        {
            PatientInfo patient = JsonMapper.ToObject<PatientInfo>(jsonData[i].ToJson());
            patientList.Add(patient);
        }
        foreach (PatientInfo patient in patientList)
        {
            GameObject button = Instantiate(buttonTemplates) as GameObject;
            button.SetActive(true);

            button.GetComponent<PatientListComponent>().SetID(patient.pid);
            button.GetComponent<PatientListComponent>().SetText("Patient name: " + patient.name.full + "\nGender: " + patient.gender + "\nDate of birth: " + patient.birthDate.Substring(0,10));

            button.transform.SetParent(buttonTemplates.transform.parent, false);
        }

        StreamReader reader = new StreamReader("./Assets/Sample/samplePatientsWithHolograms.json");
        string json = reader.ReadToEnd();

        patientList.Clear();

        JSONNode InitialJsonData = JSON.Parse(json);
        JSONArray JsonArray = InitialJsonData.AsArray;
        foreach (JSONNode PatientJson in JsonArray)
        {
            Patient patient = HoloStorageClient.JsonToPatient(PatientJson);
            if (patient.pid != null)
            {
                patientList.Add(patient);
            }
        }
    }*/

    void Start()
    {
        //ReadJsonFile();
        StartCoroutine(GetRequest("http://localhost:3001/api/v1/patients"));   
    }
}
