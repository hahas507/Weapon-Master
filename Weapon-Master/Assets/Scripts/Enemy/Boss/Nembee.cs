using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nembee : MonoBehaviour
{
    private float t = 0;
    [SerializeField] [Range(0, 20)] private float speed, walkSpeed;
    [SerializeField] [Range(0, 5)] private float atkPrepareTime;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fruit;
    [SerializeField] private GameObject[] Jakos;
    [SerializeField] [Range(1, 10)] private int spawnHowMany;
    [SerializeField] [Range(1, 15)] private int spawnRange;
    [SerializeField] [Range(1, 5)] private int fruitTimer;
    private Tree tree;
    private Vector3 playerPos;
    private Vector3 playerDir;
    private Vector3 atkDir;
    private Vector3 lookAngle;
    [SerializeField] private LayerMask obsticleMask;
    [SerializeField] private LayerMask playerMask;

    //[SerializeField] [Range(0, 20)] public float range;
    [SerializeField] [Range(0, 20)] public float followStopRange, obsDetectRange;

    [SerializeField] [Range(0, 20)] private int jumpHeight;
    [SerializeField] [Range(0, 10)] private float jumpSpeed;
    [SerializeField] private int currentAction;
    private bool alreadyInAction = false;
    [SerializeField] [Range(1, 3)] private int attackCount;
    [SerializeField] [Range(1, 5)] private float attackDelay;
    [SerializeField] [Range(1, 2)] private float attackForce;
    [SerializeField] [Range(0, 4)] private float recoverTime;

    private bool isIdle = false;

    private Rigidbody rig;

    private void Start()
    {
        tree = FindObjectOfType<Tree>();
        rig = GetComponent<Rigidbody>();
        //StartCoroutine(HopOn("Rock"));
        //StartCoroutine(ThrowFruit());
        //StartCoroutine(CallJakos());
        currentAction = 3;
    }

    private void Update()
    {
        Actions(currentAction);
    }

    public enum CURRENTACTION
    {
        IDLE = 1,
        STOP,
        FOLLOW,
        HOPON,
        THROW,
        MAKEDICISION,
        ATTACK_1,
        ATTACK_2,
        REPOSITION,
    }

    public CURRENTACTION CAction;

    private void Actions(float decisionNum)
    {
        CAction = (CURRENTACTION)decisionNum;
        switch (CAction)
        {
            case CURRENTACTION.IDLE:
                StartCoroutine(Idle());
                break;

            case CURRENTACTION.STOP:
                Stop();
                break;

            case CURRENTACTION.FOLLOW:
                Follow();
                break;

            case CURRENTACTION.MAKEDICISION:
                MakeDecision();
                break;

            case CURRENTACTION.ATTACK_1:
                Attack_1();
                break;

            case CURRENTACTION.ATTACK_2:
                Attack_2();
                break;

            case CURRENTACTION.REPOSITION:
                RePosition();
                break;

            default:
                rig.velocity = Vector3.zero * 0;
                break;
        }
    }

    private IEnumerator Idle()
    {
        Stop();
        if (!isIdle)
        {
            isIdle = true;
            recoverTime = 4;
            while (recoverTime >= 0)
            {
                recoverTime -= Time.deltaTime;
                yield return null;
            }
            isIdle = false;
            currentAction = 9;
        }
    }

    private void MakeDecision() // action 6
    {
        rig.velocity = Vector3.zero;
        GetPlayerInfo();
        float distance = (transform.position - playerPos).magnitude;
        if (distance < followStopRange * 1.5f)
        {
            currentAction = Random.Range(7, 9);
        }
        else
        {
            currentAction = 3;
        }
    }

    #region ATTACK

    private void Attack_1()
    {
        if (!alreadyInAction)
        {
            alreadyInAction = true;
            StartCoroutine(SlashAttack());
        }
    }

    private void Attack_2()
    {
        if (!alreadyInAction)
        {
            StartCoroutine(JumpTo());
        }
    }

    private IEnumerator SlashAttack()
    {
        int counter;
        counter = attackCount;
        while (counter > 0)
        {
            GetPlayerInfo();
            Vector3 launchDir;
            rig.velocity = Vector3.zero;
            transform.eulerAngles = lookAngle;
            launchDir = atkDir;

            if (launchDir.magnitude <= 1)
            {
                launchDir = transform.forward * 5;
            }
            rig.AddForce(launchDir * attackForce, ForceMode.VelocityChange);
            counter--;
            yield return new WaitForSeconds(attackDelay);
        }
        alreadyInAction = false;
        currentAction = 1;
    }

    private IEnumerator JumpTo()
    {
        alreadyInAction = true;
        yield return null;
        GetPlayerInfo();
        Vector3 startPoint = transform.position;
        Vector3 landPoint = playerPos;
        Vector3 passPoint = (((landPoint - startPoint) * .5f) + Vector3.up * jumpHeight) + startPoint;
        while (t <= 1)
        {
            t += Time.deltaTime * jumpSpeed;
            //아래는 애니메이션
            //if (t > 0 && t! < .5f)
            //{
            //    anim.SetBool("Jump", true);
            //}
            //else if (t >= .5f && t < 1)
            //{
            //    anim.SetBool("Jump", false);
            //    anim.SetBool("Landing", true);
            //}

            transform.eulerAngles = Vector3.up * (GetDegree(startPoint, landPoint));
            transform.position = new Vector3(
           ThreePointBezier(startPoint.x, passPoint.x, landPoint.x),
           ThreePointBezier(startPoint.y, passPoint.y, landPoint.y),
           ThreePointBezier(startPoint.z, passPoint.z, landPoint.z));

            if (t >= 1)
            {
                //anim.SetBool("Landing", false);
                //anim.SetTrigger("LandingAttack");
                //yield return new WaitForSeconds(attackDelay);

                rig.velocity = Vector3.zero;
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

                //anim.SetTrigger("AfterAttack");
                //yield return new WaitForSeconds(ClipDuration("AfterAttack"));

                // isJumping = false;
            }

            yield return null;
        }
        if (t >= 1) t = 0;
        currentAction = 1;
        alreadyInAction = false;
    }

    private void RePosition()
    {
        GetPlayerInfo();
        float distance = Vector3.Distance(transform.position, playerPos);

        if (distance < followStopRange)
        {
            transform.eulerAngles = -lookAngle;
            rig.AddForce(-transform.forward * speed * 1.5f, ForceMode.VelocityChange);
            if (rig.velocity.magnitude > speed)
            {
                rig.velocity = Vector3.ClampMagnitude(rig.velocity, speed);
            }
        }
        else
        {
            currentAction = 3;
        }
    }

    #endregion ATTACK

    private void Stop()
    {
        rig.velocity = Vector3.zero * 0;
    }

    private void Follow()
    {
        GetPlayerInfo();

        transform.eulerAngles = lookAngle;
        rig.AddForce(playerDir * speed, ForceMode.VelocityChange);
        if (rig.velocity.magnitude > speed)
        {
            rig.velocity = Vector3.ClampMagnitude(rig.velocity, speed);
        }
        if (Physics.Raycast(transform.position, playerDir, followStopRange, playerMask))
        {
            rig.velocity = Vector3.zero;
            currentAction = 0;
            StartCoroutine(PrepareAttack());
        }
    }

    private IEnumerator PrepareAttack()
    {
        GetPlayerInfo();
        int ranDirDicision = Random.Range(-1, 2);
        float walkTime = Random.Range(1, atkPrepareTime);
        Vector3 walkDir = lookAngle + (Vector3.up * (75 * ranDirDicision));

        if (ranDirDicision != 0)
        {
            if (!Physics.Raycast(transform.position, walkDir, obsDetectRange, obsticleMask))
            {
                float timer = walkTime;
                while (timer > 0)
                {
                    WalkToSide(ranDirDicision);
                    timer -= Time.deltaTime;
                    yield return null;
                }
                currentAction = 6;
            }
            else
            {
                float timer = walkTime;
                while (timer > 0)
                {
                    WalkToSide(ranDirDicision);
                    timer -= Time.deltaTime;
                    yield return null;
                }
                currentAction = 6;
            }
        }
        else
        {
            rig.velocity = Vector3.zero;
            GetPlayerInfo();
            transform.eulerAngles = lookAngle;
            yield return new WaitForSeconds(1.5f);

            currentAction = 6;
        }
    }

    private void WalkToSide(int dir)
    {
        GetPlayerInfo();
        transform.eulerAngles = lookAngle;
        Vector3 direction = transform.forward;
        var quaternion = Quaternion.Euler(dir * Vector3.up * 75);
        Vector3 walkDir = quaternion * direction;
        rig.AddForce(walkDir * walkSpeed, ForceMode.VelocityChange);
        if (rig.velocity.magnitude > walkSpeed)
        {
            rig.velocity = Vector3.ClampMagnitude(rig.velocity, walkSpeed);
        }
    }

    private IEnumerator HopOn(string _obsticle)
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, 40, obsticleMask);
        for (int i = 0; i < _target.Length; i++)
        {
            if (_target[i].transform.tag == _obsticle)
            {
                Vector3 targetObs = _target[i].transform.position;
                Vector3 startPoint = transform.position;
                Vector3 landPoint = targetObs + Vector3.up * 4;
                Vector3 passPoint = landPoint + Vector3.up * jumpHeight;
                while (t <= 1)
                {
                    t += Time.deltaTime * jumpSpeed;
                    transform.eulerAngles = Vector3.up * (GetDegree(startPoint, landPoint));
                    transform.position = new Vector3(
                   ThreePointBezier(startPoint.x, passPoint.x, landPoint.x),
                   ThreePointBezier(startPoint.y, passPoint.y, landPoint.y),
                   ThreePointBezier(startPoint.z, passPoint.z, landPoint.z));
                    yield return null;
                }
                if (t >= 1) t = 0;
            }
        }
    }

    private IEnumerator ThrowFruit()
    {
        float timer = 0;
        while (!tree.ISTREEDOWN)
        {
            GetPlayerInfo();
            transform.eulerAngles = lookAngle;
            timer += Time.deltaTime;
            if (timer >= fruitTimer)
            {
                GameObject clone = Instantiate(fruit, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
                clone = Instantiate(fruit, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
                clone = Instantiate(fruit, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
                clone = Instantiate(fruit, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.4f);
                clone = Instantiate(fruit, transform.position, Quaternion.identity);
                timer = 0;
            }
            yield return null;
        }
    }

    private IEnumerator CallJakos()
    {
        //after animation
        while (spawnHowMany > 0)
        {
            int spawn = Random.Range(0, Jakos.Length);
            GameObject clone = Instantiate(Jakos[spawn], transform.position + new Vector3(Random.onUnitSphere.x, 0, Random.onUnitSphere.z) * spawnRange, Quaternion.identity);
            spawnHowMany--;
            yield return new WaitForSeconds(.7f);
        }
    }

    #region math

    private float GetDegree(Vector3 _from, Vector3 _to)
    {
        return Mathf.Atan2(_from.x - _to.x, _from.z - _to.z) * Mathf.Rad2Deg;
    }

    private float ThreePointBezier(float a, float b, float c)
    {
        return b + Mathf.Pow((1 - t), 2) * (a - b) + Mathf.Pow(t, 2) * (c - b);
    }

    private float FourPointBezier(float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
          + Mathf.Pow((1 - t), 2) * 3 * t * b
          + Mathf.Pow(t, 2) * (1 - t) * 3 * c
          + Mathf.Pow(t, 3) * d;
    }

    private void GetPlayerInfo()
    {
        playerPos = player.transform.position;
        playerDir = (playerPos - transform.position).normalized;
        atkDir = playerPos - transform.position;
        lookAngle = Vector3.up * GetDegree(transform.position, playerPos);
    }

    #endregion math
}