using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIgerbeetle : EnemyControllerTest
{
    private float t;
    [SerializeField] [Range(0, 5)] private float jumpSpeed;
    [SerializeField] [Range(0, 20)] private float jumpHeight;
    [SerializeField] private GameObject point;

    protected override void Update()
    {
        base.Update();
        Search();
        if (alreadyFoundPlayer)
        {
            BattleStart();

            Follow();
            alreadyBattleStarted = false;
        }
    }

    private void FixedUpdate()
    {
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
        Vector3 landPoint = targetPosition;
        Vector3 passPoint = (((targetPosition - transform.position) * .25f) + Vector3.up * jumpHeight) + transform.position;
        Instantiate(point, passPoint, Quaternion.identity);
        while (t <= 1)
        {
            t += Time.deltaTime * jumpSpeed;
            transform.position = new Vector3(
           ThreePointBezier(transform.position.x, passPoint.x, landPoint.x),
           ThreePointBezier(transform.position.y, passPoint.y, landPoint.y),
           ThreePointBezier(transform.position.z, passPoint.z, landPoint.z));

            yield return null;
        }
        alreadyInAction = false;
    }
}