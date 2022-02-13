using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody rig;

    private bool isMoving = false;
    private Vector3 playerPos;

    public Vector3 PLAYERPOS
    {
        get { return playerPos; }
        private set { playerPos = value; }
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerPos = transform.position;
        Move();
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveDirX, 0, moveDirZ);

        if (movement == Vector3.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (isMoving)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        rig.AddForce(movement.normalized * moveSpeed, ForceMode.VelocityChange);
        if (rig.velocity.magnitude > moveSpeed)
        {
            rig.velocity = Vector3.ClampMagnitude(rig.velocity, moveSpeed);
        }
    }
}