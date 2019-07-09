using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatientListComponent : MonoBehaviour
{
    [SerializeField]
    private Text listText;
    private TextMeshProUGUI Text;

    public void SetText(string mytext)
    {
        listText.text = mytext;
    }
}
