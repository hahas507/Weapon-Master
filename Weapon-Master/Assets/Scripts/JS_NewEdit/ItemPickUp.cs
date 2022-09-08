using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private PlayerController_Enchant playerController_Enchant;

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log(equipment.equipment_Name + "ȹ��");
            playerController_Enchant.Acquire_Equipment(equipment);//��� ����Ʈ�� �ش� ��� ���� ����.
            Destroy(this.gameObject);
        }
    }
}
