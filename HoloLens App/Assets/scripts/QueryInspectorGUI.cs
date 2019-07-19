using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ConnectorQuery))]
public class QueryInspectorGUI : Editor
{
    /*private SerializedObject test;
    private SerializedProperty BaseUri, PatientInformation, HologramInformation, Patient;
    void OnEnable()
    {
        test = new SerializedObject(target);
        BaseUri = test.FindProperty("BaseUri");
        PatientInformation = test.FindProperty("PatientInformation");
        Patient = test.FindProperty("Patient");
        HologramInformation = test.FindProperty("HologramInformation");
    }
    public override void OnInspectorGUI()
    {
        //test.Update();
        EditorGUILayout.PropertyField(BaseUri);
        EditorGUILayout.PropertyField(PatientInformation);
        EditorGUILayout.PropertyField(HologramInformation);
        EditorGUILayout.PropertyField(Patient);
        test.ApplyModifiedProperties();
    }*/
}
