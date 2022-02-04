using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] protected int thisHP;
    protected int HP;
    protected bool isDead = false;

    [SerializeField] [Range(0, 100)] protected int thisATK;
    protected int ATK;

    private void Awake()
    {
        HP = thisHP;
    }

    protected void Damage(int dmg)
    {
        if (!isDead)
        {
            HP -= dmg;
            if (HP <= 0)
            {
                HP = 0;
                isDead = true;
            }
        }
    }
}