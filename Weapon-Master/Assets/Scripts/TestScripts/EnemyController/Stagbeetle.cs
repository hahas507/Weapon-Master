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

    private float afterAttackDelay;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Search();
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
        anim.SetBool("isWalking", false);
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
            anim.SetBool("prepareAttack", true);
            yield return new WaitForSeconds(attackDelay);

            anim.SetBool("prepareAttack", false);
            anim.SetTrigger("rightBefore");
            yield return new WaitForSeconds(0.5f);
            anim.Play("Attack");
            rig.velocity = Vector3.zero;
            transform.eulerAngles = lookAngle;
            rig.AddForce(targetDir * attackForce, ForceMode.VelocityChange);
            counter--;
            yield return new WaitForSeconds(attackDelay / 2);
        }
        alreadyInAction = false;
    }
}