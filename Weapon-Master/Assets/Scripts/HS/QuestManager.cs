using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData{
    public bool isActive;
    public string title;
    public string description;
    public int experienceReward;
    public int goldReward;
}

public class QuestManager : MonoBehaviour
{
    public QuestData quest;
}
