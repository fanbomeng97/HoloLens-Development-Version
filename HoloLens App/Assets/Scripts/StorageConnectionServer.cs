using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using System.Collections.Generic;
using System.Reflection;
using LitJson;


namespace HoloRepository
{
    public class StorageConnectionServer : MonoBehaviour
    {
        #region Properties
        private static string BaseUri = "http://localhost:3001/api/v1";
        private static string WebRequestReturnData = null;
        #endregion Properties

        #region Public Method
        public static void SetBaseUri(string Uri)
        {
            BaseUri = Uri;
        }

        public static IEnumerator GetAllPatient(List<PatientInfo> patientList)
        {
            string AllPatientUri = BaseUri + "/patients";
            yield return GetRequest(AllPatientUri);

            patientList.Clear();
            if (WebRequestReturnData != null)
            {
                JsonData jsonData = JsonMapper.ToObject(WebRequestReturnData);
                for (int i = 0; i < jsonData.Count; i++)
                {
                    try
                    {
                        PatientInfo patient = JsonMapper.ToObject<PatientInfo>(jsonData[i].ToJson());
                        patientList.Add(patient);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Failed to get the patient: " + e.Message);
                    }                   
                }
            }           
        }

        public static IEnumerator GetPatient(PatientInfo patient, string patientID)
        {
            string GetPatientUri = BaseUri + "/patients/" + patientID;
            yield return GetRequest(GetPatientUri);

            try
            {
                PatientInfo Patient = JsonMapper.ToObject<PatientInfo>(WebRequestReturnData);
                CopyProperties(Patient, patient);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to map the patient: " + e.Message);
            }                 
        }

        public static IEnumerator GetHologram(HoloGrams hologram, string HolgramID)
        {
            string GetHologramUri = BaseUri + "/holograms/" + HolgramID;
            yield return GetRequest(GetHologramUri);

            try
            {
                HoloGrams Hologram = JsonMapper.ToObject<HoloGrams>(WebRequestReturnData);
                CopyProperties(Hologram, hologram);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to get the Hologram: " + e.Message);
            }           
        }

        public static async void LoadHologram(string HologramID)
        {
            WebRequestReturnData = null;
            //string GetHologramUri = BaseUri + "/holograms/" + HologramID + "/download";
            string GetHologramUri = "https://holoblob.blob.core.windows.net/test/DamagedHelmet-18486331-5441-4271-8169-fcac6b7d8c29.glb";

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
        #endregion Public Method

        #region Private Common Method
        private static IEnumerator GetRequest(string uri)
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

        private static void CopyProperties(object source, object destination)
        {
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();
            foreach (PropertyInfo destinationPi in destinationProperties)
            {
                PropertyInfo sourcePi = source.GetType().GetProperty(destinationPi.Name);
                destinationPi.SetValue(destination, sourcePi.GetValue(source, null), null);
            }
        }
        #endregion Private Commom Method
    }
}