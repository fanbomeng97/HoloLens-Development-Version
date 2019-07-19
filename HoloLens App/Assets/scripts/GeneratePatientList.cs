using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;
using UnityEngine.Networking;

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
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log("Web Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":Received: " + webRequest.downloadHandler.text);
            }
            string json = webRequest.downloadHandler.text;

            JsonData jsonData = JsonMapper.ToObject(json);
            for (int i = 0; i < jsonData.Count; i++)
            {
                employee employe = JsonMapper.ToObject<employee>(jsonData[i].ToJson());
                employeeList.Add(employe);
            }
            foreach (employee patient in employeeList)
            {
                Debug.Log(patient.id);
                GameObject button = Instantiate(buttonTemplates) as GameObject;
                button.SetActive(true);

                button.GetComponent<PatientListComponent>().SetID(patient.id);
                button.GetComponent<PatientListComponent>().SetText("Patient name: " + patient.employee_name + "\nAge: " + patient.employee_age);

                button.transform.SetParent(buttonTemplates.transform.parent, false);
            }
        }
    }

    public void ReadJsonFile()
    {
        StreamReader reader = new StreamReader("./Assets/Sample/samplePatientsWithHolograms.json");
        string json = reader.ReadToEnd();

        patientList.Clear();
        JsonData jsonData = JsonMapper.ToObject(json);
        for (int i = 0; i < jsonData.Count; i++)
        {
            //PatientInfo patient = new PatientInfo { pid = "123", name = new PersonName { title = "Mr", full = "Alice White", first = "Alice", last = "White" }, gender = "female", birthDate ="1970-01-01", holograms = new HoloGrams[1] { new HoloGrams {hid = "1234", title="Lung", createdDate = "2019-07-19" } } };
            PatientInfo patient = JsonMapper.ToObject<PatientInfo>(jsonData[i].ToJson());
            patientList.Add(patient);
        }
        foreach (PatientInfo patient in patientList)
        {
            GameObject button = Instantiate(buttonTemplates) as GameObject;
            button.SetActive(true);

            button.GetComponent<PatientListComponent>().SetID(patient.pid);
            button.GetComponent<PatientListComponent>().SetText("Patient name: " + patient.name.first + " " + patient.name.last + "\nGender: " + patient.gender + "\nDate of birth: " + patient.birthDate.Substring(0,10));

            button.transform.SetParent(buttonTemplates.transform.parent, false);
        }
    }

    void Start()
    {
        ReadJsonFile();
        //StartCoroutine(GetRequest("http://dummy.restapiexample.com/api/v1/employees"));   
    }
}
