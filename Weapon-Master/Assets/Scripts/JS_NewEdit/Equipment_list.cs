using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_list : MonoBehaviour
{
    private EnchantController enchantController;

    [SerializeField]
    private ScrollRect scrollRect;

    public void Acquire_Equipment(List<Equipment> equipment)
    {
        int offset = 0;
        GameObject content = GameObject.Find("Content");

        for (int i = 0; i < equipment.Count; i++)
        {
            if (equipment[i] != null)
            {
                var weapon = Instantiate(equipment[i].image_Prefab, new Vector3(0, offset, 0), Quaternion.identity);
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
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }

}
