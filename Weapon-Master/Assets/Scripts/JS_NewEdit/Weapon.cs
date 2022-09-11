using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Weapon : MonoBehaviour
{
    [SerializeField]
    private string weaponName;

    [SerializeField]
    private int curEnchant = 0;
    [SerializeField]
    private int curAttackpt = 0;
    

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private EnchantController enchantController;

    [SerializeField]
    private GameObject image_Prefab;

    [SerializeField]
    private GameObject equip_Prefab;

    void Start()
    {
    }

    void Update()
    {
        
    }

    virtual public void GetWeapon()
    {
        enchantController = GetComponentInParent<EnchantController>();
        enchantController.GetWeapon(this);
    }

    public void SetEnchant(int _enchant)
    {
        curEnchant = _enchant;
    }

    public int GetEnchant()
    {
        return curEnchant;
    }

    public void SetAttack(int _attack)
    {
        curAttackpt = _attack;
    }

    public int GetAttack()
    {
        return curAttackpt;
    }

    public string GetName()
    {
        return weaponName;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public GameObject GetImagePrefab()
    {
        return image_Prefab;
    }
}
