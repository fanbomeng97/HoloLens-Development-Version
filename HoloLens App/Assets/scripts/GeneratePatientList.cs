using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePatientList : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplates;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject button = Instantiate(buttonTemplates) as GameObject;
            button.SetActive(true);

            //button.GetComponent<>().SetText("patient name: Alice");

            button.transform.SetParent(buttonTemplates.transform.parent, false);
        }
    }
}
