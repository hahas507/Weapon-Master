using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] public float searchRange;[System.NonSerialized] public float range;

    [Range(0, 100)]
    [SerializeField] public float battleRange;

    [SerializeField] private LayerMask targetMask;

    private bool alreadyInBattle = false;

    private Rigidbody enemyRig;
    private GameObject player;

    private void Start()
    {
        enemyRig = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Search();
    }

    private void Search() //only used for detecting player.
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, range, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform targetTransform = _target[i].transform;

            if (targetTransform.tag == "Player")
            {
                if (!alreadyInBattle)
                {
                    Debug.Log("Found player.");
                    range *= 2;

                    FollowPlayer();
                }
                alreadyInBattle = true;
                BattleStart();
            }
        }
        if (_target.Length == 0)
        {
            alreadyInBattle = false;
            range = searchRange;
            Debug.Log("Player not found.");
        }
    }

    private void FollowPlayer()
    {
        //Path find using raycast. LayerMask must include Obsticle and Player layer.
        //Stop after entering battle range.
    }

    private void BattleStart()
    {
        //need another Physics.OverlapSphere with half the size of Search()'s. Which will be used during battle.

        Collider[] _target = Physics.OverlapSphere(transform.position, battleRange, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform targetTransform = _target[i].transform;

            if (targetTransform.tag == "Player")
            {
                Debug.Log("battle start.");
                RandomActions();
            }
        }
    }

    public enum ACTIONS
    {
        IDLE,
        ATTACK1,
        ATTACK2,
        ATTACK3,
        ATTACK4, //FIGHTs are types of combat. Something like bash, slash, kick. etc.
        RETREAT, //wander around the player.
        FOLLOW,
    }

    public ACTIONS actions;

    private void RandomActions() //Chooses action randomly during battle.
    {
        int decision = Random.Range(0, System.Enum.GetValues(typeof(ACTIONS)).Length - 1); //FOLLOW is not included.
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