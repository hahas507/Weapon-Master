using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Status
{
    public static bool targetingMode = false;

    public float moveSpeed;
    public float sightRange;
    public float rotateSpeed;
    public float dodgeTimer;
    public Camera theCamera;
    public Text noEnemyText;

    float moveDirX;
    float moveDirZ;
    bool isDodge;
    bool initRotate;
    bool completeInitRotate = true;
    int targetIdx;

    Vector3 velocity;
    Vector3 targetDir;
    Vector3 targetingCameraPos = new Vector3(0.0f, 15.0f, -10.0f);
    Vector3 nonTargetingCameraPos = new Vector3(0.0f, 25.0f, -25.0f);
    GameObject[] monsters;
    GameObject targetMonster;
    SpawnManager spm;
    Rigidbody rb;
    Animator anim;
    BoxCollider boxCollider;
    List<GameObject> activeMonsters = new List<GameObject>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        spm = GameObject.Find("SpawnMgr").GetComponent<SpawnManager>();
    }

    void Update()
    {
        moveDirX = Input.GetAxisRaw("Horizontal");
        moveDirZ = Input.GetAxisRaw("Vertical");
        SetAnimParameter((int)moveDirX, (int)moveDirZ);

        if (Input.GetKeyDown(KeyCode.Space)) Dodge();

        monsters = GameObject.FindGameObjectsWithTag("Enemy"); //all monsters on field

        if (targetMonster && !IsTargetVisible(targetMonster)) //if target is out of range
        {
            targetMonster = null;
            theCamera.transform.localPosition = nonTargetingCameraPos;
            targetingMode = false;
            StartCoroutine(InitPlayerRotation());
        }

        SwitchTarget(GetInSightMonsters(monsters));
    }

    void SetAnimParameter(int x, int z)
    {
        anim.SetBool("isRun", !(x == 0 && z == 0));
        anim.SetInteger("DirX", x);
        anim.SetInteger("DirZ", z);
    }

    void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        Vector3 moveHorizontal = this.transform.right * moveDirX;
        Vector3 moveVertical = this.transform.forward * moveDirZ;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * moveSpeed;

        rb.MovePosition(this.transform.position + velocity * Time.deltaTime);
    }

    bool IsTargetVisible(GameObject target) //determine if object is in camera sight
    {
        if (!IsTargetInRange(target)) return false; //if object is out of range
        var planes = GeometryUtility.CalculateFrustumPlanes(theCamera);
        var point = target.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0) return false;
        }
        return true;
    }

    bool IsTargetInRange(GameObject target) //determine if object is in range
    {
        float dist = Vector3.Distance(target.transform.position, this.transform.position);
        if (dist < sightRange) return true;
        return false;
    }

    List<GameObject> GetInSightMonsters(GameObject[] monsters) //get objects that is both in camera sight and range
    {
        activeMonsters.Clear();
        for (int i = 0; i < monsters.Length; i++)
        {
            if (IsTargetVisible(monsters[i])) activeMonsters.Add(monsters[i]);
        }

        SortTarget(activeMonsters);
        return activeMonsters;
    }

    void SortTarget(List<GameObject> monstersLst)
    {
        monstersLst.Sort(delegate (GameObject t1, GameObject t2)
        {
            if (t1.transform.position.x.Equals(t2.transform.position.x))
            {
                return Vector3.Distance(t1.transform.position, this.transform.position).CompareTo(Vector3.Distance(t2.transform.position, this.transform.position));
            }
            return (t1.transform.position.x).CompareTo(t2.transform.position.x);
        });
    }

    void SwitchTarget(List<GameObject> targets)
    {
        if (!completeInitRotate) return;

        targetIdx = GetInSightMonsters(monsters).IndexOf(targetMonster); //get index of target
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            if (targets.Count == 0)
            {
                noEnemyText.gameObject.SetActive(true);
                StartCoroutine(TextFadeOut());
                return;
            }
            if (!targets.Contains(targetMonster) || initRotate)
            {
                targetMonster = targets[0];
                targetIdx = 0;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Q)) targetIdx = targets.IndexOf(targetMonster) - 1; //previous monster
                if (Input.GetKeyDown(KeyCode.E)) targetIdx = targets.IndexOf(targetMonster) + 1; //next monster
            }
            theCamera.transform.localPosition = targetingCameraPos;
            initRotate = false;
            targetingMode = true;
        }

        if (Input.GetKeyDown(KeyCode.P)) //quit targeting mode, init rotation of player
        {
            targetingMode = false;
            initRotate = true;
            completeInitRotate = false;
            targetMonster = null;
            theCamera.transform.localPosition = nonTargetingCameraPos;
            StartCoroutine(InitPlayerRotation());
        }

        if (targetMonster && !initRotate) //not null
        {
            targetIdx = Mathf.Clamp(targetIdx, 0, targets.Count - 1);
            targetMonster = targets[targetIdx];
            targetDir = targetMonster.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(targetDir), rotateSpeed * Time.deltaTime);
        }
    }

    IEnumerator MoveCameraPosition(Vector3 targetPos){
        while(theCamera.transform.localPosition != targetPos){
            theCamera.transform.localPosition = Vector3.Lerp(theCamera.transform.localPosition, targetPos, rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator InitPlayerRotation()
    {
        while (this.transform.rotation != Quaternion.identity)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.identity, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        completeInitRotate = true;
        initRotate = false;
    }

    IEnumerator TextFadeOut()
    {
        noEnemyText.color = new Color(noEnemyText.color.r, noEnemyText.color.g, noEnemyText.color.b, 1);
        while (noEnemyText.color.a > 0.0f)
        {
            noEnemyText.color = new Color(noEnemyText.color.r, noEnemyText.color.g, noEnemyText.color.b, noEnemyText.color.a - (Time.deltaTime / 2.0f));
            if (noEnemyText.color.a <= 0.0f) noEnemyText.gameObject.SetActive(false);
            yield return null;
        }
    }

    void Dodge()
    {
        if (!isDodge)
        {
            anim.SetTrigger("Dodge");
            StartCoroutine(DodgeDelay());
        }
    }

    IEnumerator DodgeDelay()
    {
        moveSpeed *= 2;
        isDodge = true;
        boxCollider.enabled = false; //disable collider -> invincible time
        yield return new WaitForSeconds(0.5f);
        moveSpeed *= 0.5f;
        boxCollider.enabled = true;
        yield return new WaitForSeconds(dodgeTimer);
        isDodge = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("SpawnTrigger"))
        {
            spm.SpawnEnemy();
        }

        if (collision.gameObject.CompareTag("Door"))
        {
            GameObject nextRoom = collision.gameObject.transform.parent.GetComponent<Door>().nextRoom;
            Door nextDoor = collision.gameObject.transform.parent.GetComponent<Door>().SideDoor;

            if (nextDoor.doorType == Door.DoorType.left)
            {
                Debug.Log("Left");
                Vector3 currPos = new Vector3(nextDoor.transform.position.x + 1.5f, 0.5f, nextDoor.transform.position.z);
                /*Player_mapgen.Instance.*/
                transform.position = currPos;
            }
            else if (nextDoor.doorType == Door.DoorType.right)
            {
                Debug.Log("right");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x - 1.5f, 0.5f, nextDoor.transform.position.z);
                /*Player_mapgen.Instance.*/
                transform.position = currPos;
            }
            else if (nextDoor.doorType == Door.DoorType.top)
            {
                Debug.Log("top");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x, 0.5f, nextDoor.transform.position.z - 1.5f);
                /*Player_mapgen.Instance.*/
                transform.position = currPos;
            }
            else
            {
                Debug.Log("bottom");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x, 0.5f, nextDoor.transform.position.z + 1.5f);
                /*Player_mapgen.Instance.*/
                transform.position = currPos;
            }
        }
    }
}