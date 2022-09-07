using System.IO;
using UnityEngine;

public class PlayerData
{
    public string playerName;
    public int currHP;
    public int currATK;
    public int experience;
    public int gold;

    public PlayerData() { }

    public PlayerData(string playerName, int currHP, int currATK, int experience, int gold) //constructor
    {
        this.playerName = playerName;
        this.currHP = currHP;
        this.currATK = currATK;
        this.experience = experience;
        this.gold = gold;
    }
}

public class DataManager : MonoBehaviour
{
    static DataManager instance = null;

    public PlayerData playerData = null;

    [System.NonSerialized]
    public int slotNum;
    [System.NonSerialized]
    public string inputName;
    [System.NonSerialized]
    public string path;

    public string playerName;
    public int currHP;
    public int currATK;
    public int experience;
    public int gold;

    PlayerController playerController;

    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        path = Application.streamingAssetsPath + "/slot";
    }

    public void SaveData()
    {
        playerController = FindObjectOfType<PlayerController>();
        if(inputName == null) playerData = new PlayerData(playerController.playerName, playerController.currHP, playerController.currATK, playerController.experience, playerController.gold); //load game
        else playerData = new PlayerData(inputName, playerController.currHP, playerController.currATK, playerController.experience, playerController.gold); //new game
        string data = JsonUtility.ToJson(playerData, true);
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

    void SetPlayerStatus(PlayerData player)
    {
        this.playerName = player.playerName;
        this.currHP = player.currHP;
        this.currATK = player.currATK;
        this.experience = player.experience;
        this.gold = player.gold;
    }
}