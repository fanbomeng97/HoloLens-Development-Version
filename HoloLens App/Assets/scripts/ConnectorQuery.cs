using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnectorQuery : MonoBehaviour
{
    [SerializeField]public string BaseUri = "https://holoblob.blob.core.windows.net/";
    //public bool PatientInformation = true;
    [System.Serializable]
    public class Patient
    {
        public TextMeshProUGUI TargetTextField;
        public bool id = true;
        public bool name = true;
        public bool birthDate = true;
        public bool gender = true;
        public bool email = false;
        public bool phone = false;
        
    }
    public Patient PatientInformationDisplay;
    //public bool HologramInformation = true;
    [System.Serializable]
    public class Hologram
    {
        public TextMeshProUGUI TargetTextField;
        public bool id = true;
        public bool title = true;
        public bool author = true;
        public bool createdDate = true;
        public bool subject = false;       
        public bool fileSizeInkb = false;
    }
    public Hologram HologramInformationDisplay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
