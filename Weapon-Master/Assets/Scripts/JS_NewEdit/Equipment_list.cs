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
    
    // �÷��̾ ���� ���� ��� ����Ʈ. �÷��̾ ����.
    // ���ȭ â Ȱ��ȭ �� �÷��̾ ���� ���� ��� ����� UI Viewport�� ����.
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
        var weapon = Instantiate(equipment[0].equipment_Prefab);
        weapon.transform.SetParent(GameObject.Find("Content").transform);

        //equipment_list.Add(equipment);
    }

}
