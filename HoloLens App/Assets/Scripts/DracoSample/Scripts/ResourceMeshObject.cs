using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using Microsoft.MixedReality.Toolkit.UI;

public class ResourceMeshObject : MonoBehaviour
{
	public string AssetName = "";

    IEnumerator Start()
    {
        var webRequest = UnityWebRequest.Get("https://dl.dropboxusercontent.com/s/sgfzzlm9gx3phf9/abdomenSkeleton_fastQuad05.drc.bytes?dl=0");
        yield return webRequest.SendWebRequest();
        if (!string.IsNullOrEmpty(webRequest.error))
        {
            yield break;
        }

        byte[] data = webRequest.downloadHandler.data;
        //Mesh mesh = MeshLoader.LoadMeshFromResources(data, AssetName);
        Mesh mesh = null;
        List<Mesh> meshes = new List<Mesh>();
        DracoMeshLoader dracoLoader = new DracoMeshLoader();

        int numFaces = dracoLoader.DecodeMesh(data, ref meshes);
        //int numFaces = dracoLoader.LoadMeshFromAsset(assetpath, ref meshes);

        if (numFaces > 0)
        {
            mesh = meshes[0];
        }

        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter == null)
        {
            filter = gameObject.AddComponent<MeshFilter>();
        }
        filter.mesh = mesh;

        Mesh meshSize = gameObject.GetComponentsInChildren<MeshFilter>()[0].sharedMesh;
        float Max = Math.Max(Math.Max(meshSize.bounds.size.x, meshSize.bounds.size.y), meshSize.bounds.size.z);
        float ScaleSize = 0.5f / Max;
        gameObject.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);
        gameObject.transform.position = new Vector3(meshSize.bounds.center.x * ScaleSize, -meshSize.bounds.center.y * ScaleSize, meshSize.bounds.center.z * ScaleSize + 2);
        gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        gameObject.AddComponent<BoundingBox>();
        gameObject.AddComponent<ManipulationHandler>();

        if (GetComponent<MeshRenderer>() == null)
        {
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = new Material(Shader.Find("Legacy Shaders/Diffuse")); ;
        }
    }
}
