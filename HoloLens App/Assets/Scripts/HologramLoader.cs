using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using HoloRepository;

public class HologramLoader : MonoBehaviour
{
    public enum ManipulationTypes
    {
        Scale,
        Rotate,
        MoveScale,
        MoveRotate,
        RotateScale,
        MoveRotateScale
    }
    public string uri = "https://holoblob.blob.core.windows.net/test/DamagedHelmet-18486331-5441-4271-8169-fcac6b7d8c29.glb";
    public string ModelName = "LoadedModel";
    public Vector3 ModelPosition;
    public Vector3 ModelRotation;
    public Vector3 ModelScale;
    public bool Manipulable = true;
    public ManipulationTypes ManipulationType;

    void Start()
    {

    }
}
