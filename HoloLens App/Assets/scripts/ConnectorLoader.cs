using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using Microsoft.MixedReality.Toolkit.UI;
using System;

public class ConnectorLoader : MonoBehaviour
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

    public async void Start()
    {
        Scene ModelDisplayScene = SceneManager.GetSceneByBuildIndex(TargetScene.buildIndex);
        Response response = new Response();

        try
        {
            response = await Rest.GetAsync(uri);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        if (!response.Successful)
        {
            Debug.LogError($"Failed to get glb model from {uri}");
            return;
        }

        var gltfObject = GltfUtility.GetGltfObjectFromGlb(response.ResponseData);

        try
        {
            GameObject loadedObject = await gltfObject.ConstructAsync();
            Initialize(loadedObject);
            SceneManager.MoveGameObjectToScene(loadedObject, ModelDisplayScene);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.Message}\n{e.StackTrace}");
            return;
        }

        if (gltfObject != null)
        {
            Debug.Log("Import successful");
        }
    }

    private void Initialize(GameObject gameobject)
    {
        Mesh mesh = gameobject.GetComponentsInChildren<MeshFilter>()[0].sharedMesh;
        float Max = Math.Max(Math.Max(mesh.bounds.size.x, mesh.bounds.size.y), mesh.bounds.size.z);
        float ScaleSize = 0.5f / Max;
        gameobject.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);
        gameobject.transform.position = new Vector3(mesh.bounds.center.x * ScaleSize, -mesh.bounds.center.y * ScaleSize, mesh.bounds.center.z * ScaleSize + 2);
        gameobject.transform.eulerAngles = new Vector3(0, 180, 0);
        gameobject.AddComponent<BoundingBox>();
        gameobject.AddComponent<ManipulationHandler>();
    }
}
