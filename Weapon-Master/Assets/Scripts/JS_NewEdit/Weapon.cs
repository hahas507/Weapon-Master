using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Weapon : MonoBehaviour
{
    public int curEnchant = 0;
    public int curAttackpt = 0;

    public Sprite weaponSprites;

    public EnchantController enchantController;

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

}
