using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField]
    private float hp;
    //공격 중 이동 방지 변수
    private bool isAttack = false;


    //플레이어의 애니메이터
    [SerializeField]
    private Animator anim;

    //공격 판정 오브젝트
    [SerializeField]
    private GameObject attackCheck;

    Rigidbody rb;
    CapsuleCollider capsuleCollider;

    protected SkinnedMeshRenderer meshRenderer;
    protected Color originColor;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = meshRenderer.material.color;
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        if (!isAttack)
        {
            Move();
        }
        BeforeAttack();

    }

    void FixedUpdate() {

    }

    void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * moveSpeed;

        rb.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    //공격키를 눌렀는지 체크
    private void BeforeAttack()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("ComboAttack1");
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("ComboAttack2");
        }
    }

    //공격 여부 체크. event를 통해 실행
    public void IsAttack()
    {
        isAttack = true;
    }

    //공격 판정 오브젝트 생성.event로 실행
    public void OnAttack()
    {
        attackCheck.SetActive(true);
    }

    //event를 통해 공격 여부 해제
    public void IsAttackReturn()
    {
        isAttack = false;
    }

    public virtual void TakeDamage(int damage)
    {
        //DecreaseHp(hp, damage);
    }


    //체력 감소
    public float DecreaseHp(float hp, int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            Dead(hp);
        }
        return hp;
    }

    public void Dead(float hp)
    {
        if (hp <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }

}
