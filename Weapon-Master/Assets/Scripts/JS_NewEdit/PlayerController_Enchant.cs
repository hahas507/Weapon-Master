using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_Enchant : MonoBehaviour
{
    public float moveSpeed;
    public float sightRange;
    public float rotateSpeed;
    public float dodgeTimer;
    public Camera theCamera;
    //public Text noEnemyText;

    public RaycastHit hitinfo;
    //[SerializeField]
    //private LayerMask layerMask;
    [SerializeField]
    private float checkRange;

    [SerializeField]
    private EnchantController enchantController;

    private float moveDirX;
    private float moveDirZ;
    private bool isDodge;
    private bool initRotate;
    private bool completeInitRotate = true;
    private int targetIdx;

    private bool isWindowOn;

    private Vector3 velocity;
    private GameObject[] monsters;
    private GameObject targetMonster;
    private SpawnManager spm;
    private Rigidbody rb;
    private Animator anim;
    private BoxCollider boxCollider;
    private List<GameObject> activeMonsters = new List<GameObject>();

    [SerializeField]
    private List<Equipment> equipment_list = new List<Equipment>();

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        //spm = GameObject.Find("SpawnMgr").GetComponent<SpawnManager>();
    }

    private void Update()
    {
        moveDirX = Input.GetAxisRaw("Horizontal");
        moveDirZ = Input.GetAxisRaw("Vertical");
        SetAnimParameter((int)moveDirX, (int)moveDirZ);

        //if (Input.GetKeyDown(KeyCode.Escape)) UnityEditor.EditorApplication.isPlaying = false; //quit game
        if (Input.GetKeyDown(KeyCode.Space)) Dodge();

        if (Input.GetKeyDown(KeyCode.R))
        {
            NPC_Check();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isWindowOn)
        {
            WindowDisappear();
        }

        monsters = GameObject.FindGameObjectsWithTag("Enemy"); //all monsters on field

        if (targetMonster && !IsTargetVisible(targetMonster)) //if target is out of range
        {
            targetMonster = null;
            StartCoroutine(InitPlayerRotation());
        }

        SwitchTarget(GetInSightMonsters(monsters));
    }

    //NPC 체크
    public void NPC_Check()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hitinfo, checkRange))
        {
            Debug.Log(hitinfo.transform.tag);

            if (hitinfo.transform.tag == "NPC")
            {
                WindowAppear();
            }

        }
    }

    //NPC 창 나타나기
    private void WindowAppear()
    {
        isWindowOn = true;
        Time.timeScale = 0;
        enchantController.EnchantOn(equipment_list);
    }

    public void WindowDisappear()
    {
        isWindowOn = false;
        Time.timeScale = 1f;
        enchantController.EnChantOff();
    }

    private void SetAnimParameter(int x, int z)
    {
        anim.SetBool("isRun", !(x == 0 && z == 0));
        anim.SetInteger("DirX", x);
        anim.SetInteger("DirZ", z);
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        Vector3 moveHorizontal = this.transform.right * moveDirX;
        Vector3 moveVertical = this.transform.forward * moveDirZ;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * moveSpeed;

        rb.MovePosition(this.transform.position + velocity * Time.deltaTime);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("SpawnTrigger"))
    //    {
    //        spm.SpawnEnemy();
    //    }
    //}

    private bool IsTargetVisible(GameObject target) //determine if object is in camera sight
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

    private bool IsTargetInRange(GameObject target) //determine if object is in range
    {
        float dist = Vector3.Distance(target.transform.position, this.transform.position);
        if (dist < sightRange) return true;
        return false;
    }

    private List<GameObject> GetInSightMonsters(GameObject[] monsters) //get objects that is both in camera sight and range
    {
        activeMonsters.Clear();
        for (int i = 0; i < monsters.Length; i++)
        {
            if (IsTargetVisible(monsters[i])) activeMonsters.Add(monsters[i]);
        }

        SortTarget(activeMonsters);
        return activeMonsters;
    }

    private void SortTarget(List<GameObject> monstersLst)
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

    private void SwitchTarget(List<GameObject> targets)
    {
        if (!completeInitRotate) return;

        targetIdx = GetInSightMonsters(monsters).IndexOf(targetMonster); //get index of target
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            if (targets.Count == 0)
            {
                //noEnemyText.gameObject.SetActive(true);
                //StartCoroutine(TextFadeOut());
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
            initRotate = false;
        }

        if (Input.GetKeyDown(KeyCode.Z)) //quit targeting mode, init rotation of player
        {
            initRotate = true;
            completeInitRotate = false;
            StartCoroutine(InitPlayerRotation());
        }

        if (targetMonster && !initRotate) //not null
        {
            targetIdx = Mathf.Clamp(targetIdx, 0, targets.Count - 1);
            targetMonster = targets[targetIdx];
            Vector3 dir = targetMonster.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        }
    }

    private IEnumerator InitPlayerRotation()
    {
        while (this.transform.rotation != Quaternion.identity)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.identity, 1.5f * rotateSpeed * Time.deltaTime);
            yield return null;
        }
        completeInitRotate = true;
    }

    /*private IEnumerator TextFadeOut()
    {
        //noEnemyText.color = new Color(noEnemyText.color.r, noEnemyText.color.g, noEnemyText.color.b, 1);
        while (noEnemyText.color.a > 0.0f)
        {
            noEnemyText.color = new Color(noEnemyText.color.r, noEnemyText.color.g, noEnemyText.color.b, noEnemyText.color.a - (Time.deltaTime / 2.0f));
            if (noEnemyText.color.a <= 0.0f) noEnemyText.gameObject.SetActive(false);
            yield return null;
        }
    }*/

    private void Dodge()
    {
        if (!isDodge)
        {
            anim.SetTrigger("Dodge");
            StartCoroutine(DodgeDelay());
        }
    }

    private IEnumerator DodgeDelay()
    {
        moveSpeed *= 2;
        isDodge = true;
        boxCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        moveSpeed *= 0.5f;
        boxCollider.enabled = true;
        yield return new WaitForSeconds(dodgeTimer);
        isDodge = false;
    }

    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("SpawnTrigger"))
        {
            int fnc_roomNum = collision.gameObject.transform.parent.GetComponent<Room>().roomNum;
            Debug.Log(fnc_roomNum);
            spm.SpawnEnemy(fnc_roomNum);
            collision.gameObject.SetActive(false);
        }

        if (collision.tag == "Door")
        {
            GameObject nextRoom = collision.gameObject.transform.parent.GetComponent<Door>().nextRoom;
            Door nextDoor = collision.gameObject.transform.parent.GetComponent<Door>().SideDoor;

            // 진행 방향을 알면 문제해결
            if (nextDoor.doorType == Door.DoorType.left)
            {
                Debug.Log("왼");
                Vector3 currPos = new Vector3(nextDoor.transform.position.x + 1.5f, 0.5f, nextDoor.transform.position.z);
                /*Player_mapgen.Instance.*/
                transform.position = currPos;
            }
            else if (nextDoor.doorType == Door.DoorType.right)
            {
                Debug.Log("오");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x - 1.5f, 0.5f, nextDoor.transform.position.z);
                /*Player_mapgen.Instance.*/
                transform.position = currPos;
            }
            else if (nextDoor.doorType == Door.DoorType.top)
            {
                Debug.Log("위");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x, 0.5f, nextDoor.transform.position.z - 1.5f);
                /*Player_mapgen.Instance.*/
                transform.position = currPos;
            }
            else
            {
                Debug.Log("아래");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x, 0.5f, nextDoor.transform.position.z + 1.5f);
                /*Player_mapgen.Instance.*/
                transform.position = currPos;
            }
        }
    }

    public void Acquire_Equipment(Equipment equipment)
    {
        equipment_list.Add(equipment);
    }

}