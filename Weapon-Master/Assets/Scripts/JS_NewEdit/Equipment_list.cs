using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_list : MonoBehaviour
{

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private PlayerController_Enchant player;

    public void Acquire_Equipment(List<Weapon> equipment)
    {
        int offset = 0;
        GameObject content = GameObject.Find("Content");

        for (int i = 0; i < equipment.Count; i++)
        {
            if (equipment[i] != null)
            {
                var weapon = Instantiate(equipment[i].GetImagePrefab(), new Vector3(0, offset, 0), Quaternion.identity);
                weapon.transform.SetParent(content.transform);
                offset -= 200;
            }

        }
    }

    public void Return_Equipment()
    {
        GameObject content = GameObject.Find("Content");
        int content_size = content.transform.childCount;

        for (int i = 0; i < content_size; i++)
        {
            GameObject content_ch = content.transform.GetChild(i).gameObject;
            Debug.Log(content_ch.GetComponent<Weapon>().GetEnchant());
            player.equipment_update(content_ch.GetComponent<Weapon>().GetEnchant(), content_ch.GetComponent<Weapon>().GetAttack(), i);
            Destroy(content_ch);
        }
    }

}
