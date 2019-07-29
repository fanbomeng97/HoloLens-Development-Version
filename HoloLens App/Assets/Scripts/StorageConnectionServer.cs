using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using System.Collections.Generic;
using System.Reflection;
using LitJson;
using SimpleJSON;


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
            //string AllPatientUri = BaseUri + "/patients?=";
            string AllPatientUri = BaseUri + "/patients";
            yield return GetRequest(AllPatientUri);

            patientList.Clear();
            if (WebRequestReturnData != null)
            {
                JSONNode InitialJsonData = JSON.Parse(WebRequestReturnData);
                JSONArray JsonArray = InitialJsonData.AsArray;
                foreach (JSONNode PatientJson in JsonArray)
                {
                    PatientInfo patient = JsonToPatient(PatientJson);
                    if (patient.pid != null)
                    {
                        patientList.Add(patient);
                    }                  
                }
                /*JsonData jsonData = JsonMapper.ToObject(WebRequestReturnData);
               for (int i = 0; i < patient.Count; i++)
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
                }*/
            }
        }

        public static IEnumerator GetPatient(PatientInfo patient, string patientID)
        {
            string GetPatientUri = BaseUri + "/patients/" + patientID;
            yield return GetRequest(GetPatientUri);
            try
            {
                JSONNode PatientJson = JSON.Parse(WebRequestReturnData);
                PatientInfo Patient = JsonToPatient(PatientJson);
                //PatientInfo Patient = JsonMapper.ToObject<PatientInfo>(WebRequestReturnData);
                CopyProperties(Patient, patient);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to get the patient: " + e.Message);
            }                      
        }

        public static IEnumerator GetHologram(HoloGrams hologram, string HolgramID)
        {
            string GetHologramUri = BaseUri + "/holograms/" + HolgramID;
            yield return GetRequest(GetHologramUri);
            try
            {
                JSONNode HologramJson = JSON.Parse(WebRequestReturnData);
                HoloGrams Hologram = JsonToHologram(HologramJson);
                //HoloGrams Hologram = JsonMapper.ToObject<HoloGrams>(WebRequestReturnData);
                CopyProperties(Hologram, hologram);
            }
            catch (Exception e)
            {
                Debug.Log("Failed to get the patient: " + e.Message);
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

        private static PatientInfo JsonToPatient(JSONNode Json)
        {
            PatientInfo patient = new PatientInfo();
            if (Json["pid"].Value == "")
            {
                Debug.Log("Non-Availiable patient!");
            }else
            {
                try
                {
                    patient.pid = Json["pid"].Value;

                    PersonName name = new PersonName();
                    name.title = Json["name"]["title"].Value;
                    name.full = Json["name"]["full"].Value;
                    name.first = Json["name"]["first"].Value;
                    name.last = Json["name"]["last"].Value;
                    patient.name = name;

                    patient.gender = Json["gender"].Value;
                    patient.email = Json["email"].Value;
                    patient.phone = Json["phone"].Value;
                    patient.birthDate = Json["birthDate"].Value;
                    patient.pictureUrl = Json["pictureUrl"].Value;

                    Address address = new Address();
                    address.street = Json["address"]["street"].Value;
                    address.city = Json["address"]["city"].Value;
                    address.state = Json["address"]["state"].Value;
                    address.postcode = Json["address"]["postcode"].AsInt;
                    patient.address = address;

                    /*
                    patient.pid = Json["pid"].Value;
                    patient.gender = Json["gender"].Value;
                    patient.birthDate = Json["birthDate"].Value;

                    PersonName name = new PersonName();
                    name.title = Json["name"]["title"].Value;
                    name.full = Json["name"]["full"].Value;
                    name.given = Json["name"]["given"].Value;
                    name.family = Json["name"]["family"].Value;
                    patient.name = name;
                     */

                }
                catch (Exception e)
                {
                    Debug.Log("Patient mapping error: " + e);
                }
            }            
            return patient;
        }

        private static HoloGrams JsonToHologram(JSONNode Json)
        {
            HoloGrams hologram = new HoloGrams();
            if (Json["hid"].Value == "")
            {
                Debug.Log("Non-Availiable Hologram!");
            }
            else
            {
                try
                {
                    hologram.hid = Json["hid"].Value;
                    hologram.title = Json["title"].Value;

                    Subject subject = new Subject();
                    subject.pid = Json["subject"]["pid"].Value;
                    PersonName name = new PersonName();
                    name.title = Json["subject"]["name"]["title"].Value;
                    name.full = Json["subject"]["name"]["full"].Value;
                    name.first = Json["subject"]["name"]["first"].Value;
                    name.last = Json["subject"]["name"]["last"].Value;
                    subject.name = name;

                    Author author = new Author();
                    author.aid = Json["author"]["aid"].Value;
                    PersonName AuthorName = new PersonName();
                    AuthorName.title = Json["author"]["name"]["title"].Value;
                    AuthorName.full = Json["author"]["name"]["full"].Value;
                    AuthorName.first = Json["author"]["name"]["first"].Value;
                    AuthorName.last = Json["author"]["name"]["last"].Value;
                    subject.name = AuthorName;

                    hologram.createdDate = Json["createdDate"].Value;
                    hologram.fileSizeInkb = Json["fileSizeInkb"].AsInt;
                    hologram.imagingStudySeriesId = Json["imagingStudySeriesId"].Value;

                    /*                   
                    hologram.hid = Json["hid"].Value;
                    hologram.title = Json["title"].Value;
                    hologram.description = Json["description"].Value;
                    hologram.contentType = Json["contentType"].Value;
                    hologram.fileSizeInkb = Json["fileSizeInkb"].AsInt;
                    hologram.bodySite = Json["bodySite"].Value;
                    hologram.dateOfImaging = Json["dateOfImaging"].Value;
                    hologram.creationDate = Json["creationDate"].Value;
                    hologram.creationMode = Json["creationMode"].Value;
                    hologram.creationDescription = Json["creationDescription"].Value;
                    hologram.aid = Json["aid"].Value;
                    hologram.pid = Json["pid"].Value;
                     */
                }
                catch (Exception e)
                {
                    Debug.Log("Hologram mapping error: " + e);
                }
            }            
            return hologram;
        }
        #endregion Private Commom Method
    }
}