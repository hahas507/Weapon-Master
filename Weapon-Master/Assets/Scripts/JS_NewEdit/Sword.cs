using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{ 

    public override void GetWeapon()
    {
        base.GetWeapon();
        //enchantController.GetWeapon(this.GetComponents<Weapon>());
    }
}
