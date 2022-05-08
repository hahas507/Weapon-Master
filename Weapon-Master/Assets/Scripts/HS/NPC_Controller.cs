using UnityEngine;
using UnityEngine.UI;

public class NPC_Controller : MonoBehaviour
{
    public static bool isTalking = false;

    public Transform player;
    public GameObject saveDialog;
    public GameObject questDialog;
    public Text questText;
    public Image saveSlot;
    public float minDistance;
    public int NPC_ID; //0 is save NPC id

    void Update()
    {
        float PNDistance = Vector3.Distance(this.transform.position, player.position); //distance between player and NPC
        if (PNDistance <= minDistance)
        {
            ShowDialog();
        }
    }

    void ShowDialog()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isTalking)
        {
            if (this.CompareTag("SaveNPC"))
            {
                isTalking = true;
                saveDialog.SetActive(true);
            }
            else if (this.CompareTag("QuestNPC"))
            {
                isTalking = true;
                questDialog.SetActive(true);
            }
        }
    }

    public void OnClickYes()
    {
        if (saveDialog.activeSelf)
        {
            isTalking = false;
            saveDialog.SetActive(false);
            saveSlot.gameObject.SetActive(true);
        }
        else if (questDialog.activeSelf)
        {
            isTalking = false;
            questDialog.SetActive(false);
            AcceptQuest();
        }
    }

    public void OnClcikNo()
    {
        if (saveDialog.activeSelf)
        {
            isTalking = false;
            saveDialog.SetActive(false);
        }
        else if (questDialog.activeSelf)
        {
            isTalking = false;
            questDialog.SetActive(false);
        }
    }

    void AcceptQuest()
    {
        print("Accept Quest");
    }
}