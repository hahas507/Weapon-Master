using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equipment")]
public class Equipment : ScriptableObject
{
    public string equipment_Name;
    public Sprite equipment_Image;
    public GameObject equipment_Prefab;
    public int attack;
}
