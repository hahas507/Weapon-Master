using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCheck : MonoBehaviour
{
    //����� ���� �� ������. ���Ŀ� �� ���⺰ �������� ����.
    public int damage;
    //�÷��̾� ��ǥ ����
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 attackCheckPlus;

    private void Update()
    {
        //collider�� �÷��̾� �տ� ����
        transform.position = player.transform.position + attackCheckPlus;
    }

    private void OnEnable()
    {
        StartCoroutine(Disappear());
    }



    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.2f);

        gameObject.SetActive(false);
    }

    /*
     private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Debug.Log("�ǰ�");
        }
    }*/

}
