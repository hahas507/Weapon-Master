using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equipment")]
public class Equipment : ScriptableObject
{
    [SerializeField]
    public string equipment_Name;
    [SerializeField]
    public GameObject image_Prefab;
    [SerializeField]
    public GameObject equipment_Prefab;
    [SerializeField]
    public int attack;
}
