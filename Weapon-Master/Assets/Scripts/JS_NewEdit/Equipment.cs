using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/Equipment")]
public class Equipment : ScriptableObject
{
    [SerializeField]
    private string equipment_Name;
    [SerializeField]
    public GameObject image_Prefab;
    [SerializeField]
    public GameObject equipment_Prefab;
    [SerializeField]
    private int attack;
    [SerializeField]
    private int enchant;

    public int GetAttack()
    {
        return attack;
    }

    public void SetAttack(int _attack)
    {
        attack = _attack;
        
    }

    public int GetEnchant()
    {
        return enchant;
    }

    public void SetEnchant(int _enchant)
    {
        enchant = _enchant;
    }

}
