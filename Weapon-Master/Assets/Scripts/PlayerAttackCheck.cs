using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCheck : MonoBehaviour
{
    //무기로 공격 시 데미지. 이후에 각 무기별 데미지로 변경.
    public int damage;
    //플레이어 좌표 받음
    [SerializeField]
    private GameObject player;

    private void Update()
    {
        //collider가 플레이어 앞에 생성
        transform.position = player.transform.position + new Vector3(0f, 1.5f, 1.5f);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            Debug.Log("피격");
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }


}
