using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefabSet : Singleton<RoomPrefabSet>
{
    [SerializeField]
    public Dictionary<string, GameObject> roomPrefabs = new Dictionary<string, GameObject>();
    public List<string> roomPrefabName;
    public List<GameObject> roomPrefabList;

    void Awake()
    {
        for (int i = 0; i < roomPrefabName.Count; i++)
        {
            roomPrefabs.Add(roomPrefabName[i], roomPrefabList[i]);
        }

    }

}
