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
    [SerializeField]
    private Sprite sprite;

    private void Awake()
    {
        attack = 0;
        enchant = 0;
        equipment_Name = "";
    }

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

    public Sprite GetSprite()
    {
        return sprite;
    }

    public string GetName()
    {
        return equipment_Name;
    }

}
