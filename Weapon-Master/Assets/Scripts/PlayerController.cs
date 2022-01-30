using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float sightRange;
    public float rotateSpeed;
    public float dodgeTimer;
    public Camera theCamera;
    public Text noEnemyText;
    public int maxHP;

    float moveDirX;
    float moveDirZ;
    bool isDodge;
    bool initRotate;
    bool completeInitRotate = true;
    int currHP;
    int targetIdx;

    Vector3 velocity;
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

    void Start()
    {
        currHP = maxHP;
    }

    void Update()
    {
        PlayerMove();
        if (Input.GetKeyDown(KeyCode.Escape)) UnityEditor.EditorApplication.isPlaying = false; //quit game
        if (Input.GetKeyDown(KeyCode.Space)) Dodge();

        monsters = GameObject.FindGameObjectsWithTag("Enemy"); //all monsters on field
        SwitchTarget(GetInSightMonsters(monsters));
    }

    void PlayerMove()
    {
        moveDirX = Input.GetAxisRaw("Horizontal");
        moveDirZ = Input.GetAxisRaw("Vertical");
        this.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * moveDirZ);
        this.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * moveDirX);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpawnTrigger"))
        {

            spm.SpawnEnemy();
        }
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
        if(!completeInitRotate) return;

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
            initRotate = false;
        }

        if (Input.GetKeyDown(KeyCode.Z)) //quit targeting mode, init rotation of player
        { 
            initRotate = true;
            completeInitRotate = false;
            StartCoroutine(RotateTo(Quaternion.identity));
        }

        if (targetMonster && !initRotate) //not null
        {
            targetIdx = Mathf.Clamp(targetIdx, 0, targets.Count - 1);
            targetMonster = targets[targetIdx];
            Vector3 dir = targetMonster.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
        }
    }

    IEnumerator RotateTo(Quaternion rot)
    {
        while (this.transform.rotation != rot)
        {
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rot, 1.5f * rotateSpeed * Time.deltaTime);
            yield return null;
        }
        completeInitRotate = true;
    }

    IEnumerator TextFadeOut(){
        noEnemyText.color = new Color(noEnemyText.color.r, noEnemyText.color.g, noEnemyText.color.b, 1);
        while (noEnemyText.color.a > 0.0f)
        {
            noEnemyText.color = new Color(noEnemyText.color.r, noEnemyText.color.g, noEnemyText.color.b, noEnemyText.color.a - (Time.deltaTime / 2.0f));
            if(noEnemyText.color.a <= 0.0f) noEnemyText.gameObject.SetActive(false);
            yield return null;
        }
    }

    void Dodge()
    {
        if (!isDodge && velocity != Vector3.zero)
        {
            anim.SetTrigger("Dodge");
            StartCoroutine(DodgeDelay());
        }
    }

    IEnumerator DodgeDelay()
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
}