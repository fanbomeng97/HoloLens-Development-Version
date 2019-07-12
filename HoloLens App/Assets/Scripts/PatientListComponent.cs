using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientListComponent : MonoBehaviour
{
    [SerializeField]
    private Text listText;
    [SerializeField]
    private Text PatientID;

    public void SetText(string mytext)
    {
        listText.text = mytext;
    }
    public void SetID(string text)
    {
        PatientID.text = text;
    }
}
