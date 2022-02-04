using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Nembee))]
public class NembeeEditor : Editor
{
    private void OnSceneGUI()
    {
        Nembee nembee = (Nembee)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(nembee.transform.position, Vector3.up, Vector3.forward, 360, nembee.range);
    }
}