using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment_list : MonoBehaviour
{
    private EnchantController enchantController;

    [SerializeField]
    private ScrollRect scrollRect;


    //[SerializeField]
    
    // 플레이어가 보유 중인 장비 리스트. 플레이어에 장착.
    // 장비강화 창 활성화 시 플레이어가 보유 중인 장비 목록을 UI Viewport에 전달.
    // 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Acquire_Equipment(List<Equipment> equipment)
    {
        int offset = 0;
        GameObject content = GameObject.Find("Content");

        for (int i = 0; i < equipment.Count; i++)
        {
            if (equipment[i] != null)
            {
                var weapon = Instantiate(equipment[i].equipment_Prefab, new Vector3(0, offset, 0), Quaternion.identity);
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
            Destroy(content.transform.GetChild(i));
        }
    }

}
