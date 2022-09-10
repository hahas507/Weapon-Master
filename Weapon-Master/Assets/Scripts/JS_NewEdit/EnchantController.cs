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
        InfoInit();
    }

    void Update()
    {
        
    }

    //텍스트로 무기와 강화 정보 출력.
    private void EnchantInfo(Weapon weapon)
    {
        enchantInfo[0].text = weapon.GetCurEnchant().ToString();
        enchantInfo[2].text = "강화확률 : " + (10 - weapon.GetCurEnchant()) * 10 + "%";
        enchantInfo[3].text = weapon.GetName();
    }

    public void BeforeEnchant()
    {
        if (curWeapon != null)
        {
            EnChant(curWeapon.GetComponent<Weapon>());
        }
    }

    //강화 로직
    public void EnChant(Weapon weapon)
    {
        int enchantIdx = weapon.GetCurEnchant();

        if (enchantIdx >= 9)
        {
            enchantInfo[1].text = "최대 단계입니다!";
            return;
        }

        int successNum = Random.Range(0, 100);

        if (successNum <= ((10-enchantIdx)*10))
        {
            enchantIdx++;
            enchantInfo[1].text = "성공!";
        }
        else
        {
            enchantInfo[1].text = "실패!";
        }

        weapon.SetCurEnchant(enchantIdx);
        EnchantInfo(weapon);

    }

    IEnumerator EnchantText()
    {
        enchantInfo[1].color = new Color(enchantInfo[1].color.r, enchantInfo[1].color.g, enchantInfo[1].color.b, 1);
        while (enchantInfo[1].color.a > 0.1f)
        {
            Debug.Log("감소");
            enchantInfo[1].color = new Color(enchantInfo[1].color.r, enchantInfo[1].color.g, enchantInfo[1].color.b, enchantInfo[1].color.a - (Time.deltaTime / 3.0f));
            yield return null;
        }
        StartCoroutine(EnchantText());

    }

    public void InfoInit()
    {
        for (int i = 0; i < enchantInfo.Length; i++)
        {
            enchantInfo[i].text = "";
        }
        selected_Image.color = new Color(255, 255, 255, 0);
    }

    public void GetWeapon(Weapon weapon)
    {
        InfoInit();
        curWeapon = weapon.gameObject;
        selected_Image.sprite = weapon.GetSprite();
        selected_Image.color = new Color(255, 255, 255, 255);
        EnchantInfo(weapon);
    }

    public void EnchantOn(List<Equipment> equipment)
    { 
        panels[0].SetActive(true);
        equipment_List.Acquire_Equipment(equipment);
    }

    public void EnChantOff()
    {
        equipment_List.Return_Equipment();
        InfoInit();
        panels[0].SetActive(false);

    }

}
