using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SelectSaveSlot : MonoBehaviour
{
    public Text[] slotText;

    bool[] saveFile = new bool[] { false, false, false };

    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            for (int i = 0; i < 3; i++)
            {
                if (File.Exists(DataManager.Instance.path + $"{i}"))
                {
                    saveFile[i] = true;
                    DataManager.Instance.slotNum = i;
                    DataManager.Instance.LoadData();
                    slotText[i].text = DataManager.Instance.playerData.playerName;
                }
            }
            DataManager.Instance.DataClear();
        }
    }

    public void UpdateSlot(int num)
    {
        DataManager.Instance.slotNum = num;
        DataManager.Instance.SaveData();
        this.gameObject.SetActive(false);
    }
}
