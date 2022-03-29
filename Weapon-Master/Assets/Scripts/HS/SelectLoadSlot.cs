using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class SelectLoadSlot : MonoBehaviour
{
    public Text[] slotText;

    bool[] saveFile = new bool[3];

    void Start()
    {
        for(int i=0;i<3;i++){
            if(File.Exists(DataManager.Instance.path + $"{i}")){
                saveFile[i] = true;
                DataManager.Instance.slotNum = i;
                DataManager.Instance.LoadData();
                slotText[i].text = DataManager.Instance.playerData.playerName;
            }
        }
        DataManager.Instance.DataClear();
    }

    public void UpdateSlot(int num){
        DataManager.Instance.slotNum = num;
        if(saveFile[num]) {
            DataManager.Instance.LoadData();
            SceneManager.LoadScene("QuestNSaveScene");
        }
    }
}
