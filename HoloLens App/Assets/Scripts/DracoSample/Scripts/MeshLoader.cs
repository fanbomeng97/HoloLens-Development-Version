using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MeshLoader : MonoBehaviour
{
    public static Mesh LoadMeshFromResources(byte[] data, string assetpath)
	{     
        if(assetpath == null || assetpath.Equals(""))
        {
            Debug.Log("Assetpath is null or empty.");
            return null;
        }

        Mesh mesh = null;

        string extension = System.IO.Path.GetExtension(assetpath);
        if(extension.Equals(".drc"))
        {
            List<Mesh> meshes = new List<Mesh>();
    		DracoMeshLoader dracoLoader = new DracoMeshLoader();

            int numFaces = dracoLoader.DecodeMesh(data, ref meshes);
            //int numFaces = dracoLoader.LoadMeshFromAsset(assetpath, ref meshes);

            if (numFaces > 0)
            {
                mesh = meshes[0];
            }           
        }
        else
        {
            mesh = (Mesh)Resources.Load(assetpath, typeof(Mesh));
        }

        return mesh;
	}
}