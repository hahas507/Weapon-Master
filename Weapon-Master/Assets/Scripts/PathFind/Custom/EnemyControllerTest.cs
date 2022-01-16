using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyControllerTest : MonoBehaviour
{
    [SerializeField] private float speed;

    [Range(0, 100)]
    [SerializeField] public float searchRange;[System.NonSerialized] public float range;//need to change to private.

    [Range(0, 100)]
    [SerializeField] public float battleRange;

    [SerializeField] private LayerMask searchMask;
    [SerializeField] private LayerMask obsticleMask;

    private bool alreadyFoundPlayer = false;
    private bool alreadyBattleStarted = false;
    private bool alreadyInAction = false;

    private RaycastHit raycastHit;
    private Vector3 targetDir;
    private Vector3 lookAngle;
    private Transform targetTransform;

    private Rigidbody rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        range = searchRange;
    }

    private void Update()
    {
        Test();
    }

    private void Test()
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, range, searchMask);
        for (int i = 0; i < _target.Length; i++)
        {
            targetTransform = _target[i].transform;

            if (targetTransform.tag == "Player")
            {
                targetDir = (targetTransform.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, targetDir, out raycastHit, range, obsticleMask))
                {
                    alreadyFoundPlayer = false;
                    range = searchRange;
                    return;
                }

                if (!alreadyFoundPlayer)
                {
                    range = searchRange * 2;
                }
                alreadyFoundPlayer = true;

                Follow();
                BattleStart();
            }
            else
            {
                alreadyFoundPlayer = false;
            }
        }
        if (!alreadyFoundPlayer)
        {
            range = searchRange;
            rig.velocity = Vector3.zero;
        }
    }

    private void Follow()
    {
        if (alreadyFoundPlayer && !alreadyBattleStarted)
        {
            lookAngle = Vector3.up * GetDegree(transform.position, targetTransform.position);
            transform.eulerAngles = lookAngle; //need to change to coroutine since it face toward the target at instant.

            rig.AddForce(targetDir * speed, ForceMode.VelocityChange);
            if (rig.velocity.magnitude > speed)
            {
                rig.velocity = Vector3.ClampMagnitude(rig.velocity, speed);
            }
        }
    }

    private float GetDegree(Vector3 _from, Vector3 _to)
    {
        return Mathf.Atan2(_from.x - _to.x, _from.z - _to.z) * Mathf.Rad2Deg;
    }

    private void BattleStart()
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, battleRange, searchMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform targetTransform = _target[i].transform;

            if (targetTransform.tag == "Player")
            {
                if (!alreadyBattleStarted)
                {
                    rig.velocity = Vector3.zero;
                }
                RandomActions();
                alreadyBattleStarted = true;
            }
        }
        if (_target.Length == 0)
        {
            alreadyBattleStarted = false;
        }
    }

    public enum ACTIONS
    {
        IDLE,
        ATTACK1,
        ATTACK2,
        ATTACK3,
        ATTACK4, //FIGHTs are types of combat. Something like bash, slash, kick. etc.
        RETREAT,
    }

    public ACTIONS actions;

    private void RandomActions()
    {
        int decision = Random.Range(0, System.Enum.GetValues(typeof(ACTIONS)).Length); //FOLLOW is not included.
        actions = (ACTIONS)decision;

        switch (actions)
        {
            case ACTIONS.ATTACK1:
                Attack1();
                break;

            case ACTIONS.ATTACK2:
                //Attack2();
                break;

            case ACTIONS.ATTACK3:
                //Attack2();
                break;

            case ACTIONS.ATTACK4:
                //Attack2();
                break;

            case ACTIONS.RETREAT:
                Retreat();
                break;
        }
    }

    private void Attack1()
    {
    }

    private void Retreat()
    {
    }
}