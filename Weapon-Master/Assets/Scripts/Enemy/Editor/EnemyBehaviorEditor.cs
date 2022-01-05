using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyBehavior))]
public class EnemyBehaviorEditor : Editor
{
    private void OnSceneGUI() //Shows the range(FOV) in the scene view.
    {
        EnemyBehavior behavior = (EnemyBehavior)target;
        Handles.color = Color.red;
        Handles.DrawWireArc(behavior.transform.position, Vector3.up, Vector3.forward, 360, behavior.searchRange);
    }
}