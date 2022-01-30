using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField] 
    public float hp;
    [SerializeField]
    private int damage;

    [Range(0, 100)]
    [SerializeField] public float searchRange;
    [System.NonSerialized] public float range;//need to change to private.

    [Range(0, 100)]
    [SerializeField] public float battleRange;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obsticleMask;

    protected bool alreadyFoundPlayer = false;
    protected bool alreadyBattleStarted = false;
    protected bool alreadyInAction = false;

    private RaycastHit raycastHit;
    protected Vector3 targetDir;
    protected Vector3 lookAngle;
    protected Vector3 targetPosition;
    private GameObject player;
    protected float clipTime;

    private List<Transform> detectedTargets;

    protected Rigidbody rig;
    protected Animator anim;
    protected RuntimeAnimatorController ac;

    protected SkinnedMeshRenderer meshRenderer;
    protected Color originColor;


    protected virtual void Awake()
    {
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        range = searchRange;
        ac = anim.runtimeAnimatorController;
    }

    protected virtual void Update()
    {
    }

    protected virtual void Search()
    {
        detectedTargets = new List<Transform>();
        Collider[] _target = Physics.OverlapSphere(transform.position, range, playerMask);
        for (int i = 0; i < _target.Length; i++)
        {
            detectedTargets.Add(_target[i].transform);
        }
        if (detectedTargets.Contains(player.transform))
        {
            Vector3 playerPos = ((detectedTargets.Find(item => item.tag == "Player").position));
            targetDir = (playerPos - transform.position).normalized;
            if (Physics.Raycast(transform.position, targetDir, range, obsticleMask))
            {
                alreadyFoundPlayer = false; // 나중에 "기척을 감지함"으로 바꿔줄 수 있음.
                range = searchRange;
            }
            else
            {
                alreadyFoundPlayer = true;
                range = searchRange * 2;
                targetPosition = playerPos;
                lookAngle = Vector3.up * GetDegree(transform.position, targetPosition);
            }
        }
        else
        {
            alreadyFoundPlayer = false;
            range = searchRange;
        }

        if (!alreadyFoundPlayer || detectedTargets.Count == 0)
        {
            range = searchRange;
        }
    }

    protected void Follow()
    {
        if (alreadyFoundPlayer)
        {
            transform.eulerAngles = lookAngle; //need to change to coroutine since it face toward the target at instant.
            if (!alreadyBattleStarted)
            {
                anim.SetBool("isWalking", true);
                rig.AddForce(targetDir * speed, ForceMode.VelocityChange);
                if (rig.velocity.magnitude > speed)
                {
                    rig.velocity = Vector3.ClampMagnitude(rig.velocity, speed);
                }
            }
        }
        else return;
    }

    protected float GetDegree(Vector3 _from, Vector3 _to)
    {
        return Mathf.Atan2(_from.x - _to.x, _from.z - _to.z) * Mathf.Rad2Deg;
    }

    protected void BattleStart()
    {
        if (Physics.Raycast(transform.position + Vector3.up, targetDir, out raycastHit, battleRange, playerMask))
        {
            float distance = (raycastHit.transform.position - transform.position).magnitude;

            if (distance < battleRange)
            {
                alreadyBattleStarted = true;
                if (!alreadyInAction)
                {
                    alreadyInAction = true;
                    JakoAttack();
                }
            }
            else alreadyBattleStarted = false;
        }
    }

    protected float ClipDuration(string clipName)
    {
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == clipName)
            {
                clipTime = ac.animationClips[i].length;
            }
        }
        return clipTime;
    }

    protected abstract void JakoAttack();


    //적이 피격받았을 때 데미지 받음
    public virtual void TakeDamage(int damage)
    {
        Debug.Log(damage + "체력 감소");
        hp = DecreaseHp(hp, damage);
        //피격받았음을 나타내는 빨간색 효과
        StartCoroutine(OnHitColor());
    }

    protected IEnumerator OnHitColor()
    {
        meshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.5f);
        Debug.Log(hp);
        meshRenderer.material.color = originColor;
    }

    //체력 감소
    public float DecreaseHp(float hp, int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Dead(hp);
        }
        return hp;
    }

    protected virtual void Dead(float hp)
    {
        if (hp <= 0)
        {
            anim.SetTrigger("Dead");
            Destroy(gameObject, 0.5f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("플레이어");
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    //public enum ACTIONS
    //{
    //    IDLE,
    //    ATTACK1,
    //    ATTACK2,
    //    ATTACK3,
    //    ATTACK4, //FIGHTs are types of combat. Something like bash, slash, kick. etc.
    //    RETREAT,
    //}

    //public ACTIONS actions;

    //private void RandomActions()
    //{
    //    int decision = Random.Range(0, System.Enum.GetValues(typeof(ACTIONS)).Length); //FOLLOW is not included.
    //    actions = (ACTIONS)decision;

    //    switch (actions)
    //    {
    //        case ACTIONS.ATTACK1:
    //            //Attack1();
    //            break;

    //        case ACTIONS.ATTACK2:
    //            //Attack2();
    //            break;

    //        case ACTIONS.ATTACK3:
    //            //Attack2();
    //            break;

    //        case ACTIONS.ATTACK4:
    //            //Attack2();
    //            break;

    //        case ACTIONS.RETREAT:
    //            //Retreat();
    //            break;
    //    }
    //}
}