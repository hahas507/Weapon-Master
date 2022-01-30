using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private const float minPathUpdateTime = .2f;
    private const float pathUpdateMoveThreshold = .5f;
    [SerializeField] private float speed = 3;

    [Range(0, 100)]
    [SerializeField] public float searchRange;[System.NonSerialized] public float range;//need to change to private. It's  public because of EnemybehaviorEditor.cs

    [Range(0, 100)]
    [SerializeField] public float battleRange;

    [SerializeField] private LayerMask SearchMask;
    [SerializeField] private LayerMask planeMask;

    private bool alreadyFoundPlayer = false;
    private bool alreadyBattleStarted = false;

    private GameObject player;
    private Vector3[] path;
    private int targetIndex;

    private RaycastHit raycastHit;
    private Vector3 targetDir;
    private Grid grid;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(GridUpdate());
    }

    private void Update()
    {
        Search();
    }

    private IEnumerator GridUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.up * -1, out raycastHit, 10, planeMask))
        {
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down, Color.red);
            if (raycastHit.transform.tag == "Plane")
            {
                Debug.Log("Plane found.");
                grid = raycastHit.transform.GetComponent<Grid>();
                grid.CreateGrid();
            }
        }
        yield return new WaitForSeconds(1f);
    }

    private void Search()
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, range, SearchMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform targetTransform = _target[i].transform;

            if (targetTransform.tag == "Player")
            {
                //targetDir = (targetTransform.position - transform.position).normalized;

                //if (Physics.Raycast(transform.position, targetDir, out raycastHit, range, targetMask))
                //{
                //    if (raycastHit.transform.tag == "Player")
                //    {
                //        Debug.DrawRay(transform.position, targetDir, Color.red);
                //        Debug.Log("Looking at Player.");
                //    }
                //}
                if (!alreadyFoundPlayer)
                {
                    range *= 2;
                    StartCoroutine(UpdatePath());
                }
                alreadyFoundPlayer = true;
                BattleStart();
            }
        }
        if (_target.Length == 0)
        {
            alreadyFoundPlayer = false;
            range = searchRange;
        }
    }

    private IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(transform.position, player.transform.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = player.transform.position;

        yield return 0;
        //Debug.Log("alreadyFoundPlayer: " + alreadyFoundPlayer);
        while (alreadyFoundPlayer)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((player.transform.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, player.transform.position, OnPathFound);
                targetPosOld = player.transform.position;
            }
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (alreadyFoundPlayer && !alreadyBattleStarted)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void BattleStart()
    {
        //need another Physics.OverlapSphere with half the size of Search()'s. Which will be used during battle.

        Collider[] _target = Physics.OverlapSphere(transform.position, battleRange, SearchMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform targetTransform = _target[i].transform;

            if (targetTransform.tag == "Player")
            {
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

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}