using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField]
    private PlayerController_Enchant playerController_Enchant;

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerController_Enchant.Acquire_Equipment(this.gameObject.GetComponent<Weapon>());//��� ����Ʈ�� �ش� ��� ���� ����.
            Destroy(this.gameObject);
        }
    }
}
