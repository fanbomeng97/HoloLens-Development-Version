using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using System.Collections.Generic;
using LitJson;

namespace HoloRepository
{
    public class StorageConnectionServer : MonoBehaviour
    {
        private static string BaseUri = "http://localhost:3001/api/v1";
        private static string WebRequestReturnData = null;

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

        public static IEnumerator GetAllPatient(List<employee> patientList)
        {
            //string AllPatientUri = BaseUri + "/patients";
            string AllPatientUri = "http://dummy.restapiexample.com/api/v1/employees";
            yield return GetRequest(AllPatientUri);
            patientList.Clear();
            JsonData jsonData = JsonMapper.ToObject(WebRequestReturnData);
            for (int i = 0; i < jsonData.Count; i++)
            {
                employee patient = JsonMapper.ToObject<employee>(jsonData[i].ToJson());
                patientList.Add(patient);
            }
        }

        public static IEnumerator GetPatient(PatientInfo patient, string patientID)
        {
            string GetPatientUri = BaseUri + "/patients/" + patientID;
            yield return GetRequest(GetPatientUri);
            patient = JsonMapper.ToObject<PatientInfo>(WebRequestReturnData);
        }

        public static IEnumerator GetHologram(HoloGrams hologram, string HolgramID)
        {
            string GetHologramUri = BaseUri + "/holograms/" + HolgramID;
            yield return GetRequest(GetHologramUri);
            hologram = JsonMapper.ToObject<HoloGrams>(WebRequestReturnData);
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

        public static IEnumerator GetRequest(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();
                WebRequestReturnData = null;
                if (webRequest.isNetworkError)
                {
                    Debug.Log("Web Error: " + webRequest.error);
                }
                else
                {
                    WebRequestReturnData = webRequest.downloadHandler.text;
                }
            }
        }
    }
}
