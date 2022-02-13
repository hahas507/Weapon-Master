using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{
    //�� ũ��
    public int Width;
    public int Height;

    //�� ��ǥ
    public Vector3 currPos;
    public Vector3 parentPos;
    public Vector3 CenterPos;

    //�� ����
    public string roomName;
    public string roomType;
    public string roomId;
    public int linked_num; //0 : ����, 1 : ������, 2 : ��, 3 : �Ʒ�
    public bool isParent;



    //
    public bool updatedWalls = false;
    public bool visitedRoom = false;
    public bool isInRoute = false;
    public GameObject Door;
    public GameObject Wall;

     

    public Room(int x, int y, int z)
    {
        currPos.x = x;
        currPos.y = y;
        currPos.z = z;
    }

    public SubRoom rooms;
    public RoomInfo roomInfo;

    public bool updateRooms = false;


    // Room�� ���� �� �ʱ⿡ ȣ��(Start)
    public void setUpdateWalls(bool setup)
    {
        updatedWalls = setup;
    }

    void Start()
    {
        //null ���� ����
        if (RoomManager.Instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        //RoomManager�� �ڽ����� ����
        rooms = GetComponentInChildren<SubRoom>();

        //RoomManager�� ������ �� ���� ����
        if (rooms != null)
        {
            rooms.currPos = currPos;
            rooms.roomType = roomType;
            rooms.Width = Width;
            rooms.Height = Height;
            rooms.roomName = roomName;
            rooms.parentPos = parentPos;
            rooms.CenterPos = CenterPos;
            rooms.isParent = isParent;
            rooms.linked_num = linked_num;
        }

        updatedWalls = false;
    }

    public void CreateDoors()
    {
        if (rooms != null)
            rooms.CreateDoors();
    }

    void Update()
    {
        if (!updatedWalls)
        {

            CreateDoors();

            updatedWalls = true;
        }
    }


    public Vector3 GetRoomCenter()
    {
        return new Vector3(currPos.x, 0, currPos.z);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            RoomManager.Instance.OnPlayerEnterRoom(this);
        }
    }

}


