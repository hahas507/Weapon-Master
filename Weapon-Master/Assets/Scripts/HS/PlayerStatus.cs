using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Range(0, 100)]
    public int currHP;
    protected int maxHP = 100;

    [Range(0, 100)]
    public int currATK;
    protected int defaultATK = 5;

    [System.NonSerialized]
    public string playerName;

    public bool isDead;

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

    public void SetPlayerStatus(string name, int HP, int ATK){
        this.playerName = name;
        this.currHP = HP;
        this.currATK = ATK;
    }
}
