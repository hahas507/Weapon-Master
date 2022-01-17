using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public int maxHP;

    float moveDirX;
    float moveDirZ;

    int currHP;

    SpawnManager spawnMgr;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        spawnMgr = GameObject.Find("SpawnMgr").GetComponent<SpawnManager>();
    }

    void Start()
    {
        currHP = maxHP;
    }

    void Update()
    {
        moveDirX = Input.GetAxisRaw("Horizontal");
        moveDirZ = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Escape)) UnityEditor.EditorApplication.isPlaying = false;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * moveSpeed;

        rb.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpawnTrigger"))
        {

            spawnMgr.SpawnEnemy();
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Enemy")){
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }
}
