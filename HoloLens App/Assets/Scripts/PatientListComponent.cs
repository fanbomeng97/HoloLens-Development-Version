using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using HoloRepository;

public class PatientListComponent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Patientinfo = null;
    [SerializeField]
    private Text PatientID = null;
    [SerializeField]
    private RawImage img = null;
    public static List<HoloGrams> HologramsList = new List<HoloGrams>();

    private void Start()
    {
        Patientinfo = GetComponent<TextMeshProUGUI>();
    }

    public void SetImage(string gender)
    {
        Texture2D tex = null;
        byte[] fileData;
        if (gender == "female")
        {
            fileData = File.ReadAllBytes("./Assets/Sample/girl.png");
        }
        else
        {
            fileData = File.ReadAllBytes("./Assets/Sample/boy.png");
        }
        tex = new Texture2D(2, 2);
        tex.LoadImage(fileData); 
        img.texture = tex;
    } 

    public void SetText(string Patientinformation)
    {
        Patientinfo.text = Patientinformation;
    }
    public void SetID(string id)
    {
        PatientID.text = id;
    }

    public void TurnToHologramPage()
    {
        HologramsList.Clear();
        foreach (PatientInfo patient in GeneratePatientList.patientList)
        {
            if (patient.pid == PatientID.text)
            {
                foreach (HoloGrams hologram in patient.holograms)
                {
                    HologramsList.Add(hologram);
                }
            }
        }
        SceneManager.UnloadSceneAsync(2);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}
