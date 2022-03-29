using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Range(0, 100)]
    public int currHP;
    int maxHP = 100;

    [Range(0, 100)]
    public int currATK;
    int maxATK = 100;

    public bool isDead;

    void Start()
    {
        currHP = maxHP;
        currATK = 1;
    }

    public virtual void Damage(int dmg)
    {
        if (!isDead)
        {
            currHP -= dmg;
            if (currHP <= 0)
            {
                currHP = 0;
                isDead = true;
            }
        }
    }

    public int GetCurrentHP()
    {
        return currHP;
    }

    public int GetCurrentATK()
    {
        return currATK;
    }
}
