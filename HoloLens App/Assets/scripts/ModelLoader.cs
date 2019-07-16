using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Microsoft.MixedReality.Toolkit.Examples.Demos.Gltf
{

    public class ModelLoader : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This can be a local or external resource uri.")]
        private string uri = "https://dl.dropboxusercontent.com/s/uqfzst339hsyosf/500_abdomen_190mb.glb";
        [SerializeField, TooltipAttribute("Enter the build index of scene that you want to load the 3D model")]
        private int SceneIndex = 2;

        public async void Start()
        {
            Scene ModelDisplayScene = SceneManager.GetSceneByBuildIndex(SceneIndex);
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
               loadedObject.transform.position = new Vector3(0.19f, -0.25f, 1.65f);
               loadedObject.transform.localScale = new Vector3(0.001F, 0.001F, 0.001F);
               loadedObject.transform.eulerAngles = new Vector3(0, 0, 0);
               loadedObject.AddComponent<BoundingBox>();
               loadedObject.AddComponent<ManipulationHandler>();
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
    }
}