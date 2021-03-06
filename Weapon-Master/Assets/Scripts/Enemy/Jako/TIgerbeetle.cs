using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIgerbeetle : EnemyController
{
    private float t;
    [SerializeField] [Range(0, 5)] private float jumpSpeed;
    [SerializeField] [Range(0, 20)] private float jumpHeight;
    [SerializeField] [Range(0, 5)] private int jumpPrepareTime;
    [SerializeField] [Range(0, 3)] private float attackDelay;
    [SerializeField] [Range(0, 10)] private int randomJumpRange;
    private bool isJumping = false;

    private void BoolDebug()
    {
        Debug.Log("alreadyFoundPlayer: " + alreadyFoundPlayer);
        Debug.Log("alreadyBattleStarted: " + alreadyBattleStarted);
        Debug.Log("alreadyInAction: " + alreadyInAction);
    }

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
        if (alreadyFoundPlayer && t == 0)
        {
            BattleStart();
            if (t == 0 && !isJumping)
            {
                Follow();
                alreadyBattleStarted = false;
            }
            else if (isJumping)
            {
                transform.eulerAngles = lookAngle;
                rig.velocity = Vector3.zero;
            }
        }
    }

    protected override void JakoAttack()
    {
        StartCoroutine(JumpTo());
    }

    private float ThreePointBezier(float a, float b, float c)
    {
        return b + Mathf.Pow((1 - t), 2) * (a - b) + Mathf.Pow(t, 2) * (c - b);
    }

    private IEnumerator JumpTo()
    {
        isJumping = true;
        anim.SetTrigger("JumpPose");
        yield return new WaitForSeconds(ClipDuration("JumpPose"));
        anim.SetTrigger("JumpPrepare");
        yield return new WaitForSeconds(jumpPrepareTime);
        anim.SetTrigger("RightBeforeJump");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Jump");
        Vector3 startPoint = transform.position;
        Vector3 landPoint = (new Vector3(Random.onUnitSphere.x, 0, Random.onUnitSphere.z) * randomJumpRange) + targetPosition;
        Vector3 passPoint = (((landPoint - startPoint) * .25f) + Vector3.up * jumpHeight) + startPoint;

        while (t <= 1)
        {
            t += Time.deltaTime * jumpSpeed;
            if (t > 0 && t! < .5f)
            {
                anim.SetBool("Jump", true);
            }
            else if (t >= .5f && t < 1)
            {
                anim.SetBool("Jump", false);
                anim.SetBool("Landing", true);
            }
            transform.eulerAngles = Vector3.up * (GetDegree(startPoint, landPoint));
            transform.position = new Vector3(
           ThreePointBezier(startPoint.x, passPoint.x, landPoint.x),
           ThreePointBezier(startPoint.y, passPoint.y, landPoint.y),
           ThreePointBezier(startPoint.z, passPoint.z, landPoint.z));

            if (t >= 1)
            {
                anim.SetBool("Landing", false);
                anim.SetTrigger("LandingAttack");
                yield return new WaitForSeconds(attackDelay);
                rig.velocity = Vector3.zero;
                anim.SetTrigger("AfterAttack");
                yield return new WaitForSeconds(ClipDuration("AfterAttack"));
                isJumping = false;
            }

            yield return null;
        }
        if (t >= 1) t = 0;
        alreadyInAction = false;
    }
}