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
        SerializedProperty eje = serializedObject.FindProperty("eje");
        EditorGUILayout.PropertyField(eje);
        SerializedProperty ejeMinimo = serializedObject.FindProperty("ejeMinimo");
        EditorGUILayout.PropertyField(ejeMinimo);
        SerializedProperty ejeMaximo = serializedObject.FindProperty("ejeMaximo");
        EditorGUILayout.PropertyField(ejeMaximo);
        SerializedProperty porciento = serializedObject.FindProperty("porciento");
        EditorGUILayout.PropertyField(porciento);
        SerializedProperty targets = serializedObject.FindProperty("targets");
        EditorGUILayout.PropertyField(targets, true);
        serializedObject.ApplyModifiedProperties();
    }
}