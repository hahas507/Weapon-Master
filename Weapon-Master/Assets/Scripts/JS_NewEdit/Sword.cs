using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void GetWeapon()
    {
        base.GetWeapon();
        //enchantController.GetWeapon(this.GetComponents<Weapon>());
    }
}
