using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnchantController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] panels;

    [SerializeField]
    private Text[] enchantInfo;

    [SerializeField]
    private GameObject curWeapon;

    [SerializeField]
    private Image selected_Image;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private Equipment_list equipment_List;

    void Start()
    {
        TextInit();
        selected_Image.color = new Color(255, 255, 255, 0);
    }

    void Update()
    {
        
    }

    //�ؽ�Ʈ�� ��ȭ �ܰ� ǥ��. �Ŀ� �̸��� ǥ��
    private void EnchantInfo(Weapon weapon)
    {
        enchantInfo[0].text = weapon.curEnchant.ToString();
        enchantInfo[2].text = "��ȭȮ�� : " + (10 - weapon.curEnchant) * 10 + "%";
    }

    public void BeforeEnchant()
    {
        if (curWeapon != null)
        {
            EnChant(curWeapon.GetComponent<Weapon>());
        }
    }

    //��ȭ ����
    public void EnChant(Weapon weapon)
    {
        int enchantIdx = weapon.curEnchant;

        if (enchantIdx >= 9)
        {
            enchantInfo[1].text = "�ִ� �ܰ��Դϴ�!";
            //StartCoroutine(EnchantText());
            return;
        }

        int successNum = Random.Range(0, 100);

        if (successNum <= ((10-enchantIdx)*10))
        {
            enchantIdx++;
            enchantInfo[1].text = "����!";
        }
        else
        {
            enchantInfo[1].text = "����!";
        }

        //StartCoroutine(EnchantText());
        weapon.curEnchant = enchantIdx;
        EnchantInfo(weapon);

    }

    IEnumerator EnchantText()
    {
        enchantInfo[1].color = new Color(enchantInfo[1].color.r, enchantInfo[1].color.g, enchantInfo[1].color.b, 1);
        while (enchantInfo[1].color.a > 0.1f)
        {
            Debug.Log("����");
            enchantInfo[1].color = new Color(enchantInfo[1].color.r, enchantInfo[1].color.g, enchantInfo[1].color.b, enchantInfo[1].color.a - (Time.deltaTime / 3.0f));
            yield return null;
        }
        StartCoroutine(EnchantText());

    }

    public void TextInit()
    {
        enchantInfo[1].text = "";
    }

    public void GetWeapon(Weapon weapon)
    {
        curWeapon = weapon.gameObject;
        selected_Image.sprite = weapon.weaponSprites;
        selected_Image.color = new Color(255, 255, 255, 255);
        TextInit();
        EnchantInfo(weapon);
    }

    public void EnchantOn(List<Equipment> equipment)
    {
        //panels[0].SetActive(false);
        panels[1].SetActive(true);
        equipment_List.Acquire_Equipment(equipment);

        
    }

    public void EnChantOff()
    {
        //equipment_List.Return_Equipment();
        panels[1].SetActive(false);

    }



}
