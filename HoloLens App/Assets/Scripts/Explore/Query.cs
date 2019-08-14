using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Query : MonoBehaviour
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
    public Scene TargetScene;
    public string ModelName = "LoadedModel";
    public Vector3 ModelPosition;
    public Vector3 ModelRotation;
    public Vector3 ModelScale;
    public bool Manipulable = true;
    public ManipulationTypes ManipulationType;
    private string uri = "https://dl.dropboxusercontent.com/s/uqfzst339hsyosf/500_abdomen_190mb.glb";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
