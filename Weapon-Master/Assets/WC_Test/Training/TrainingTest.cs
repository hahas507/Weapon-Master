using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTest : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float speedLimit;
    Vector3 playerMove;
    private float applySpeed;
    Rigidbody rig;

    void Start(){
        rig = GetComponent<Rigidbody>();
    }

    void Update(){
        Move();
    }
    
    void Move(){
        applySpeed = playerSpeed;
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        playerMove = Vector3.ClampMagnitude(new Vector3(xInput, 0f, zInput),1f);
        rig.MovePosition(transform.position + playerMove * Time.deltaTime * applySpeed);
    }

    void Attack(){

    }   
}
