using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField InputField = null;

    public void One()
    {
        InputField.text = InputField.text + "1";
    }
    public void Two()
    {
        InputField.text = InputField.text + "2";
    }
    public void Three()
    {
        InputField.text = InputField.text + "3";
    }
    public void Four()
    {
        InputField.text = InputField.text + "4";
    }
    public void Five()
    {
        InputField.text = InputField.text + "5";
    }
    public void Six()
    {
        InputField.text = InputField.text + "6";
    }
    public void Seven()
    {
        InputField.text = InputField.text + "7";
    }
    public void Eight()
    {
        InputField.text = InputField.text + "8";
    }
    public void Nine()
    {
        InputField.text = InputField.text + "9";
    }
    public void Zero()
    {
        InputField.text = InputField.text + "0";
    }
    public void Delete()
    {
        if (InputField.text.Length > 0)
        {
            InputField.text = InputField.text.Substring(0, InputField.text.Length-1);
        }       
    }
    public void Appear()
    {
        gameObject.SetActive(true);
    }
    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}
