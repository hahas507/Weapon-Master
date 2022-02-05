using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    //���� �������� �̸�
    public string currentWorldName = "Basement";

    public GameObject roomPrefab;


    public RoomInfo currentLoadRoomData;
    public Room currRoom; //���� ��

    public List<Room> loadedRooms = new List<Room>(); //��鿡 ���� ����Ʈ
    public GameObject roomPrefabs;
    public bool isLoadingRoom = false;
    public bool spawnedBossRoom = false;
    public bool updatedRooms = false;
    public bool createRoom = false;

    //�� �ʱ�ȭ. ���ο� �� ����� ����
    public void newCreatedRoom()
    {
        isLoadingRoom = false;
        spawnedBossRoom = false;
        updatedRooms = false;
        createRoom = false;

        //��� �ڽ� ��ü �ı�
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //�ʱ�ȭ
        loadedRooms.Clear();

        //�÷��̾� ��ġ �ʱ�ȭ(ó����ġ��)
        Player_mapgen.Instance.transform.position = new Vector3(0, 0.5f, 0);
        //�� ����
        DungeonCreator.Instance.DungeonCreate();

        UpdateRoomQueue();


    }

    //����Ʈ�� ���� ����(���� ��)
    void UpdateRoomQueue()
    {
        //���ѻ��� ����
        if (isLoadingRoom)
        {
            return;
        }


        if (loadedRooms.Count > 0)
        {
            foreach (Room room in loadedRooms)
            {
                room.CreateDoors();
            }
            isLoadingRoom = true; //�ε���(���ѻ��� ����)
        }
    }

    //DungeonCrawlerController���� ���� �� �迭�� ���޹���
    public void LoadRoom(RoomInfo oldRoom)
    {
        //�� ��ħ ����
        if (DoesRoomExist(oldRoom.currPos.x, oldRoom.currPos.y, oldRoom.currPos.z))
        {
            return;
        }

        string roomPreName = oldRoom.roomName;
        //�� ������ ����
        GameObject room = Instantiate(RoomPrefabSet.Instance.roomPrefabs[roomPreName]);
        //�� ��ġ
        room.transform.position = new Vector3(
                    (oldRoom.currPos.x * room.transform.GetComponent<Room>().Width * 10),
                     oldRoom.currPos.y,
                    (oldRoom.currPos.z * room.transform.GetComponent<Room>().Height * 10)
        );
        //�� ���� ũ��
        room.transform.localScale = new Vector3(
                    (room.transform.GetComponent<Room>().Width),
                     1,
                    (room.transform.GetComponent<Room>().Height));


        //��Ÿ ������. ��ġ, �̸�, Ÿ�� ���.
        room.transform.GetComponent<Room>().currPos = oldRoom.currPos;
        room.name = currentWorldName + "-" + oldRoom.roomName + " " + oldRoom.currPos.x + ", " + oldRoom.currPos.z;
        room.transform.GetComponent<Room>().roomName = oldRoom.roomName;
        room.transform.GetComponent<Room>().roomId = oldRoom.roomID;
        room.transform.GetComponent<Room>().isInRoute = oldRoom.isInRoute;
        room.transform.GetComponent<Room>().isParent = oldRoom.isLinked;
        room.transform.GetComponent<Room>().linked_num = oldRoom.room_linked_dir;

        //RoomController ������Ʈ�� �ڽ� ��ü�� ����
        room.transform.parent = transform;
        //����Ʈ�� ���� �ο�
        loadedRooms.Add(room.GetComponent<Room>());
    }

    // �� �ߺ� ����
    public bool DoesRoomExist(float x, float y, float z)
    {
        return loadedRooms.Find(item => item.currPos.x == x && item.currPos.y == y && item.currPos.z == z) != null;
    }

    //���ǿ� �´� �� ã��
    public Room FindRoom(float x, float y, float z)
    {
        // List.Find : item ���� ���ǿ� �´� Room�� ã�� ��ȯ
        return loadedRooms.Find(item => item.currPos.x == x && item.currPos.y == y && item.currPos.z == z);
    }

    // �ش� Room���� Player�� �ִ� ���� ��ȯ
    public void OnPlayerEnterRoom(Room room)
    {
        currRoom = room;

    }

}
