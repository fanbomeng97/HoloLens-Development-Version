using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using HoloRepository.ModelSetting;
using System.Collections.Generic;
using LitJson;

public class StorageConnectionServer : MonoBehaviour
{
    private static string BaseUri = "http://localhost:3001/api/v1";
    private static string WebRequestReturnData = null;
    private static List<PatientInfo> patientList = new List<PatientInfo>();
    private static PatientInfo patient = new PatientInfo();

    #region Sigleton
    private static StorageConnectionServer mInstance = null;
    private static StorageConnectionServer instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType(typeof(StorageConnectionServer)) as StorageConnectionServer;

                if (mInstance == null)
                {
                    mInstance = new GameObject("StaticCoroutine").AddComponent<StorageConnectionServer>();
                }
            }
            return mInstance;
        }
    }
    #endregion Sigleton

    #region MonoBehaviour Method
    void Awake()
    {
        if (mInstance == null)
        {
            mInstance = this as StorageConnectionServer;
        }
    }
    void Die()
    {
        mInstance = null;
        Destroy(gameObject);
    }
    void OnApplicationQuit()
    {
        mInstance = null;
    }
    #endregion MonoBehaviour Method

    public static void SetBaseUri(string Uri)
    {
        BaseUri = Uri;
    }

    public static List<PatientInfo> GetAllPatient()
    {
        string AllPatientUri = BaseUri + "/patients";
        instance.StartCoroutine(instance.GetRequest(AllPatientUri, (result) => {
            string json = result;
            patientList.Clear();
            JsonData jsonData = JsonMapper.ToObject(json);
            for (int i = 0; i < jsonData.Count; i++)
            {
                PatientInfo patient = JsonMapper.ToObject<PatientInfo>(jsonData[i].ToJson());
                patientList.Add(patient);
            }
        }));
        return patientList;
    }

    public static PatientInfo GetPatient(string patientID)
    {
        WebRequestReturnData = null;
        string GetPatientUri = BaseUri + "/patients/" + patientID;
        instance.StartCoroutine(instance.GetRequest(GetPatientUri, (result) => {
            WebRequestReturnData = result;
            patient = JsonMapper.ToObject<PatientInfo>(result);
        }));
        return patient;
    }

    public static async void LoadHologram(string HologramID)
    {
        WebRequestReturnData = null;
        string GetHologramUri = BaseUri + "/holograms/" + HologramID + "/download";

        Response response = new Response();
        try
        {
            response = await Rest.GetAsync(GetHologramUri);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        if (!response.Successful)
        {
            Debug.LogError($"Failed to get glb model from {GetHologramUri}");
            return;
        }

        var gltfObject = GltfUtility.GetGltfObjectFromGlb(response.ResponseData);

        try
        {
            GameObject loadedObject = await gltfObject.ConstructAsync();
            ModelSetting.Initialize(loadedObject);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
            return;
        }    
    }

    IEnumerator GetRequest(string uri, Action<string> result)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Web Error: " + webRequest.error);
                result("Web Error: " + webRequest.error);
            }
            else
            {
                result?.Invoke(webRequest.downloadHandler.text);             
            }
        }
    }
}
