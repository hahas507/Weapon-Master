using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
    public InputField playerNameInput;

    string playerName = "";
    bool isRightName = false;

    void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    void Update()
    {
        playerName = playerNameInput.text;
        if (playerName.Length > 1 && playerName.Length <= 10 && !playerName.Contains(" "))
        {
            isRightName = true;
        }

        else isRightName = false;
    }

    public void SetName()
    {
        if (!isRightName) print("inappropriate name");
        else if (PlayerPrefs.HasKey(playerName)) print("name is already exist.");
        else
        {
            PlayerPrefs.SetInt(playerName, 1);
            this.gameObject.SetActive(false);
            DataManager.Instance.inputName = playerName;
            SceneManager.LoadScene("QuestNSaveScene");
        }
    }
}
