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
            Debug.Log(equipment.equipment_Name + "획득");
            playerController_Enchant.Acquire_Equipment(equipment);//장비 리스트에 해당 장비 정보 전달.
            Destroy(this.gameObject);
        }
    }
}
