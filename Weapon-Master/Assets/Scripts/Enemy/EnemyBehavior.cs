using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] public float searchRange;

    [SerializeField] private LayerMask targetMask;

    private bool alreadyInBattle = false;

    private Rigidbody enemyRig;

    private void Start()
    {
        enemyRig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Search();
    }

    private void Search() //only used for detecting player.
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, searchRange, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform targetTransform = _target[i].transform;

            if (targetTransform.tag == "Player")
            {
                Debug.Log("Player found.");

                if (!alreadyInBattle)
                {
                    BattleStart();
                }
                alreadyInBattle = true;
            }
        }
    }

    private void BattleStart()
    {
        //FollowPlayer();
        //need another Physics.OverlapSphere with half the size of Search()'s. Which will be used during battle.
    }

    public enum ACTIONS
    {
        ATTACK1,
        ATTACK2,
        ATTACK3,
        ATTACK4, //FIGHTs are types of combat. Something like bash, slash, kick. etc.
        RETREAT, //wander around the player.
    }

    public ACTIONS actions;

    private void RandomActions() //Chooses action randomly during battle.
    {
        int decision = Random.Range(0, System.Enum.GetValues(typeof(ACTIONS)).Length + 1);
        actions = (ACTIONS)decision;

        switch (actions)
        {
            case ACTIONS.ATTACK1:
                Attack1();
                break;

            case ACTIONS.ATTACK2:
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