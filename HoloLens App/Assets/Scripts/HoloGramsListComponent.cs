using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HoloRepository;

public class HoloGramsListComponent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI HologramInfo = null;
    [SerializeField]
    private Text HologramID = null;
    public static string HologramId;

    private void Start()
    {
        HologramInfo = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string HologramInformation)
    {
        HologramInfo.text = HologramInformation;
    }
    public void SetID(string id)
    {
        HologramID.text = id;
    }

    public void DisplayModel()
    {
        ModelSetting.SetManipulable(true);
        ModelSetting.SetRotation(new Vector3(0, 180, 0));
        ModelSetting.SetPostition(new Vector3(0f, 0f, 2f));
        ModelSetting.SetSize(0.5f);
        ModelSetting.SetSeceneName("ModelDisplayScene");
        StorageConnectionServer.LoadHologram("hololensid");
    }
}
