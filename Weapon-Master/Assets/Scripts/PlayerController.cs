using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    [SerializeField]
    private float hp;
    //���� �� �̵� ���� ����
    private bool isAttack = false;


    //�÷��̾��� �ִϸ�����
    [SerializeField]
    private Animator anim;

    //���� ���� ������Ʈ
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

    //����Ű�� �������� üũ
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

    //���� ���� üũ. event�� ���� ����
    public void IsAttack()
    {
        isAttack = true;
    }

    //���� ���� ������Ʈ ����.event�� ����
    public void OnAttack()
    {
        attackCheck.SetActive(true);
    }

    //event�� ���� ���� ���� ����
    public void IsAttackReturn()
    {
        isAttack = false;
    }

    public virtual void TakeDamage(int damage)
    {
        //DecreaseHp(hp, damage);
    }


    //ü�� ����
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
