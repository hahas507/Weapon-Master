using System.IO;
using UnityEngine;

public class PlayerData
{
    public string playerName;
    public int currHP;
    public int currATK;

    public PlayerData() { }

    public PlayerData(string playerName, int currHP, int currATK) //constructor
    {
        this.playerName = playerName;
        this.currHP = currHP;
        this.currATK = currATK;
    }
}

public class DataManager : MonoBehaviour
{
    static DataManager instance = null;
    public PlayerData playerData;

    [System.NonSerialized]
    public int slotNum;
    [System.NonSerialized]
    public string inputName;
    [System.NonSerialized]
    public string path;

    public string playerName;
    public int currHP;
    public int currATK;

    PlayerController playerController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
        path = Application.persistentDataPath + "/slot";
    }

    public static DataManager Instance
    {
        get
        {
            if (!instance) return null;
            return instance;
        }
    }

    public void SaveData()
    {
        playerController = FindObjectOfType<PlayerController>();
        if(inputName == null) playerData = new PlayerData(playerController.playerName, playerController.currHP, playerController.currATK);
        else playerData = new PlayerData(inputName, playerController.currHP, playerController.currATK);
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(path + slotNum.ToString(), data);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + slotNum.ToString());
        playerData = JsonUtility.FromJson<PlayerData>(data);
        SetPlayerStatus(playerData);
    }

    public void DataClear()
    {
        slotNum = -1;
        playerData = new PlayerData();
    }

    void SetPlayerStatus(PlayerData player){
        this.playerName = player.playerName;
        this.currHP = player.currHP;
        this.currATK = player.currATK;
    }
}