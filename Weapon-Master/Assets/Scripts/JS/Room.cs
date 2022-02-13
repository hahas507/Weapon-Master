using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room : MonoBehaviour
{
    //방 크기
    public int Width;
    public int Height;

    //방 좌표
    public Vector3 currPos;
    public Vector3 parentPos;
    public Vector3 CenterPos;

    //방 정보
    public string roomName;
    public string roomType;
    public string roomId;
    public int linked_num; //0 : 왼쪽, 1 : 오른쪽, 2 : 위, 3 : 아래
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


    // Room을 생성 시 초기에 호출(Start)
    public void setUpdateWalls(bool setup)
    {
        updatedWalls = setup;
    }

    void Start()
    {
        //null 오류 방지
        if (RoomManager.Instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        //RoomManager를 자식으로 받음
        rooms = GetComponentInChildren<SubRoom>();

        //RoomManager에 생성된 방 정보 전달
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


