using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nembee : MonoBehaviour
{
    private float t = 0;
    [SerializeField] [Range(0, 20)] private float speed;
    private RaycastHit raycastHit;
    [SerializeField] private GameObject player;
    private Vector3 playerPos;
    private Vector3 playerDir;
    private Vector3 lookAngle;
    [SerializeField] private LayerMask obsticleMask;
    [SerializeField] [Range(0, 20)] private float range;
    [SerializeField] [Range(0, 20)] private int jumpHeight;
    [SerializeField] [Range(0, 10)] private float jumpSpeed;
    [SerializeField] [Range(0, 2)] private int currentAction;

    private Rigidbody rig;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
    }

    public enum CURRENTACTION
    {
        STOP,
        FOLLOW,
    }

    public CURRENTACTION CAction;

    private void Actions()
    {
        CAction = (CURRENTACTION)currentAction;
        switch (CAction)
        {
            case CURRENTACTION.FOLLOW:
                Follow();
                break;

            case CURRENTACTION.STOP:
                // Do nothing.
                break;

            default:
                Follow();
                break;
        }
    }

    private void MakeDecision()
    {
        GetPlayerInfo();
        if (!Physics.Raycast(transform.position + Vector3.up, playerDir, out raycastHit, range, obsticleMask))
        {
            //currentAction = 1;
        }
        else
        {
            //그냥 navmesh쓰자
        }
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

    protected float GetDegree(Vector3 _from, Vector3 _to)
    {
        return Mathf.Atan2(_from.x - _to.x, _from.z - _to.z) * Mathf.Rad2Deg;
    }

    private float ThreePointBezier(float a, float b, float c)
    {
        return b + Mathf.Pow((1 - t), 2) * (a - b) + Mathf.Pow(t, 2) * (c - b);
    }

    private void GetPlayerInfo()
    {
        playerPos = player.transform.position;
        playerDir = (playerPos - transform.position).normalized;
        lookAngle = Vector3.up * GetDegree(transform.position, playerPos);
    }
}