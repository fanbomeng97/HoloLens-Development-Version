using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using System;

namespace HoloRepository.ModelSetting
{
    public class ModelSetting : MonoBehaviour
    {
        #region Properties
        private static string ModelName = "LoadedModel";
        private static Vector3 ModelPosition = new Vector3(0f, 0f, 0f);
        private static Vector3 ModelRotation = new Vector3(0f, 0f, 0f);
        private static float ModelSize = 1f;
        private static bool Manipulable = true;
        #endregion Properties

        #region Set Method
        public static void SetModelName(string name)
        {
            ModelName = name;
        }

        public static void SetPostition(Vector3 position)
        {
            ModelPosition = position;
        }

        public static void SetRotation(Vector3 rotation)
        {
            ModelRotation = rotation;
        }

        public static void SetSize(float size)
        {
            ModelSize = size;
        }

        public static void SetManipulable(bool manipulable)
        {
            Manipulable = manipulable;
        }
        #endregion Set Method

        public static void Initialize(GameObject gameobject)
        {
            Mesh mesh = gameobject.GetComponentsInChildren<MeshFilter>()[0].sharedMesh;
            float Max = Math.Max(Math.Max(mesh.bounds.size.x, mesh.bounds.size.y), mesh.bounds.size.z);

            float ScaleSize = ModelSize / Max;
            gameobject.transform.localScale = new Vector3(ScaleSize, ScaleSize, ScaleSize);

            Vector3 InitialPosition = new Vector3(mesh.bounds.center.x , -mesh.bounds.center.y, mesh.bounds.center.z) * ScaleSize;
            gameobject.transform.position = InitialPosition + ModelPosition;

            gameobject.transform.eulerAngles = ModelRotation;

            if (Manipulable)
            {
                gameobject.AddComponent<BoundingBox>();
                gameobject.AddComponent<ManipulationHandler>();
            }
        }
    }
}


