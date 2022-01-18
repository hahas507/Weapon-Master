using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyControllerTest), true), CanEditMultipleObjects]
public class EnemyControllerTestEditor : Editor
{
    private void OnSceneGUI() //Shows the range(FOV) in the scene view.
    {
        EnemyControllerTest behavior = (EnemyControllerTest)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(behavior.transform.position, Vector3.up, Vector3.forward, 360, behavior.searchRange);
        Handles.color = Color.red;
        Handles.DrawWireArc(behavior.transform.position, Vector3.up, Vector3.forward, 360, behavior.range);
        Handles.color = Color.blue;
        Handles.DrawWireArc(behavior.transform.position, Vector3.up, Vector3.forward, 360, behavior.battleRange);
    }
}