using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagbeetle : EnemyController
{
    [Range(0, 30)]
    [SerializeField] private float attackForce;

    [Range(0, 5)]
    [SerializeField] private int attackCount;

    private int counter;

    [Range(0, 3)]
    [SerializeField] private float attackDelay;

    private Vector3 launchDir;

    private void Update()
    {
        if (!isDead)
        {
            Search();
        }
        else return;
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
            rig.velocity = Vector3.zero;
            anim.SetBool("prepareAttack", true);
            yield return new WaitForSeconds(attackDelay);

            anim.SetBool("prepareAttack", false);
            anim.SetTrigger("rightBefore");
            transform.eulerAngles = lookAngle;
            launchDir = targetDir;
            yield return new WaitForSeconds(0.5f);
            anim.Play("Attack");
            rig.velocity = Vector3.zero;

            rig.AddForce(launchDir * attackForce, ForceMode.VelocityChange);
            counter--;
            yield return new WaitForSeconds(attackDelay / 2);
        }
        alreadyInAction = false;
    }
}