using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class WebRequestHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("This can be a local or external resource uri.")]
    private string baseUri = "http://localhost:3001/api/v1/patients";

    void Start()
    {
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
    }
}
