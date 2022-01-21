using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    float moveDirX;
    float moveDirZ;

    SpawnManager spm;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        spm = GameObject.Find("SpawnMgr").GetComponent<SpawnManager>();
    }

    void Start()
    {

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

            spm.SpawnEnemy();
        }
    }
}
