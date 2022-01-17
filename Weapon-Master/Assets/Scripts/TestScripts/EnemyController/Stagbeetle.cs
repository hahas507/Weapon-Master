using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagbeetle : EnemyControllerTest
{
    [Range(0, 20)]
    [SerializeField] private float attackForce;

    [Range(0, 5)]
    [SerializeField] private int attackCount;

    private int counter;

    [Range(0, 3)]
    [SerializeField] private float attackDelay;

    protected override void Update()
    {
        base.Update();
        Search();
        Debug.Log(alreadyBattleStarted);
    }

    protected override void Search()
    {
        base.Search();
        if (alreadyFoundPlayer && counter == 0)
        {
            BattleStart();
            if (counter == 0)
            {
                Follow();
                alreadyBattleStarted = false;
            }
        }
    }

    protected override void JakoAttack()
    {
        StopCoroutine(SlashAttack());
        if (counter == 0)
        {
            StartCoroutine(SlashAttack());
        }
    }

    private IEnumerator SlashAttack()
    {
        counter = attackCount;
        while (counter > 0)
        {
            yield return new WaitForSeconds(attackDelay);
            rig.velocity = Vector3.zero;
            transform.eulerAngles = lookAngle;
            rig.AddForce(targetDir * attackForce, ForceMode.VelocityChange);
            counter--;
        }
        alreadyInAction = false;
    }
}