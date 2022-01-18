using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIgerbeetle : EnemyControllerTest
{
    private float t;
    [SerializeField] [Range(0, 5)] private float jumpSpeed;
    [SerializeField] [Range(0, 20)] private float jumpHeight;
    [SerializeField] [Range(0, 5)] private int jumpPrepareTime;
    [SerializeField] private GameObject point;

    protected override void Update()
    {
        base.Update();
        Search();
        if (alreadyFoundPlayer)
        {
            BattleStart();

            Follow();
        }
        BoolDebug();
    }

    private void BoolDebug()
    {
        Debug.Log("alreadyFoundPlayer: " + alreadyFoundPlayer);
        Debug.Log("alreadyBattleStarted: " + alreadyBattleStarted);
        Debug.Log("alreadyInAction: " + alreadyInAction);
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
        anim.SetTrigger("JumpPose");
        yield return new WaitForSeconds(ClipDuration("JumpPose"));
        anim.SetTrigger("JumpPrepare");
        yield return new WaitForSeconds(jumpPrepareTime);
        anim.SetTrigger("RightBeforeJump");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Jump");
        Vector3 startPoint = transform.position;
        Vector3 landPoint = targetPosition;
        Vector3 passPoint = (((targetPosition - startPoint) * .25f) + Vector3.up * jumpHeight) + startPoint;

        Instantiate(point, passPoint, Quaternion.identity);
        while (t <= 1)
        {
            t += Time.deltaTime * jumpSpeed;
            transform.position = new Vector3(
           ThreePointBezier(startPoint.x, passPoint.x, landPoint.x),
           ThreePointBezier(startPoint.y, passPoint.y, landPoint.y),
           ThreePointBezier(startPoint.z, passPoint.z, landPoint.z));

            yield return null;
        }
        if (t >= 1) t = 0;
        alreadyInAction = false;
    }
}