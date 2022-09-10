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

    public void SetCurEnchant(int _enchant)
    {
        curEnchant = _enchant;
    }

    public int GetCurEnchant()
    {
        return curEnchant;
    }

    public void SetCurAttackpt(int _attack)
    {
        curAttackpt = _attack;
    }

    public int GetCurAttackpt()
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
}
