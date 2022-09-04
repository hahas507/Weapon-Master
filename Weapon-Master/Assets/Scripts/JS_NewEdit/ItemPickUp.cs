using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private Equipment_list equipment_List;

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log(equipment.equipment_Name + "ȹ��");
            equipment_List.Acquire_Equipment(equipment);//��� ����Ʈ�� �ش� ��� ���� ����.
            Destroy(this.gameObject);
        }
    }
}
