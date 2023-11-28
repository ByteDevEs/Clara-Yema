using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
[CustomEditor(typeof(CamaraCambioDeAngulo))]
public class CamaraCambioDeAnguloEditor : Editor
{
    
    public override void OnInspectorGUI () {
        serializedObject.Update();
        SerializedProperty angulos = serializedObject.FindProperty("angulos");
        EditorGUILayout.PropertyField(angulos, true);
        SerializedProperty constraintMinimo = serializedObject.FindProperty("constraintMinimo");
        EditorGUILayout.PropertyField(constraintMinimo);
        SerializedProperty constraintMaximo = serializedObject.FindProperty("constraintMaximo");
        EditorGUILayout.PropertyField(constraintMaximo);
        SerializedProperty targets = serializedObject.FindProperty("targets");
        EditorGUILayout.PropertyField(targets, true);
        serializedObject.ApplyModifiedProperties();
    }
}