﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginAuth : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField PIN;
    [SerializeField]
    private TextMeshProUGUI WarningInfo;
    [SerializeField]
    private GameObject PINPad;

    private void Start()
    {
        TMP_InputField PIN = GetComponent<TMP_InputField>();
    }

    public void Auth()
    {
        if (PIN.text == "1234")
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            ScenesManager.RemoveComponent(PINPad);
        }
        else
        {
            WarningInfo.text = "The PIN is wrong!";
            PIN.Select();
            PIN.text = "";
        }
    }

    public void Clear()
    {
        WarningInfo.text = "";
        PIN.Select();
        PIN.text = "";  
    }
}
