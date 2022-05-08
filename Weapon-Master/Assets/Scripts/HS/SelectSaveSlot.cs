using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SelectSaveSlot : MonoBehaviour
{
    public Text[] slotText;

    bool[] saveFile = new bool[3];

    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            for (int i = 1; i < 4; i++)
            {
                if (File.Exists(DataManager.Instance.path + $"{i}"))
                {
                    saveFile[i - 1] = true;
                    DataManager.Instance.slotNum = i;
                    DataManager.Instance.LoadData();
                    slotText[i - 1].text = DataManager.Instance.playerData.playerName;
                }
            }
            DataManager.Instance.DataClear();
        }
    }

    public void UpdateSlot(int num)
    {
        DataManager.Instance.slotNum = num + 1;
        DataManager.Instance.SaveData();
        this.gameObject.SetActive(false);
    }
}
