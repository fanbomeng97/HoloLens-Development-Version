using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class WebRequestHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("This can be a local or external resource uri.")]
    private string baseUri = "http://dummy.restapiexample.com/api/v1/employees";

    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest(baseUri));
    }

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
        }

        WWW www = new WWW("http://dummy.restapiexample.com/api/v1/employees");
        yield return www;
        if (www.error != null)
        {
            print("WWW Error: " + www.error);
            yield break;
        }
        string json = www.text;

        List<employee> list = new List<employee>();
        JsonData jsonData = JsonMapper.ToObject(json);
        for (int i = 0; i < jsonData.Count; i++)
        {
            employee employe = JsonMapper.ToObject<employee>(jsonData[i].ToJson());
            list.Add(employe);
        }
        foreach (employee employe in list)
        {
            Debug.Log(employe.id + " " + employe.employee_name);
        }
    }

    [System.Serializable]
    public class employee
    {
        public string id;
        public string employee_name;
        public string emloyee_salary;
        public string employee_age;
        public string profile_image;

        /*public static PatientInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PatientInfo>(jsonString);
        }*/
    }

}
