using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    //던전 스테이지 이름
    public string currentWorldName = "Basement";

    public GameObject roomPrefab;


    public RoomInfo currentLoadRoomData;
    public Room currRoom; //현재 방

    public List<Room> loadedRooms = new List<Room>(); //방들에 대한 리스트
    public GameObject roomPrefabs;
    public bool isLoadingRoom = false;
    public bool spawnedBossRoom = false;
    public bool updatedRooms = false;
    public bool createRoom = false;

    //방 초기화. 새로운 맵 만들기 위함
    public void newCreatedRoom()
    {
        isLoadingRoom = false;
        spawnedBossRoom = false;
        updatedRooms = false;
        createRoom = false;

        //모든 자식 객체 파괴
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //초기화
        loadedRooms.Clear();

        //플레이어 위치 초기화(처음위치로)
        Player_mapgen.Instance.transform.position = new Vector3(0, 0.5f, 0);
        //맵 생성
        DungeonCreator.Instance.DungeonCreate();

        UpdateRoomQueue();


    }

    //리스트에 정보 삽입(문과 벽)
    void UpdateRoomQueue()
    {
        //무한생성 막음
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
            isLoadingRoom = true; //로딩중(무한생성 방지)
        }
    }

    //DungeonCrawlerController에서 만든 방 배열을 전달받음
    public void LoadRoom(RoomInfo oldRoom)
    {
        //방 겹침 방지
        if (DoesRoomExist(oldRoom.currPos.x, oldRoom.currPos.y, oldRoom.currPos.z))
        {
            return;
        }

        string roomPreName = oldRoom.roomName;
        //방 프리팹 생성
        GameObject room = Instantiate(RoomPrefabSet.Instance.roomPrefabs[roomPreName]);
        //방 위치
        room.transform.position = new Vector3(
                    (oldRoom.currPos.x * room.transform.GetComponent<Room>().Width * 10),
                     oldRoom.currPos.y,
                    (oldRoom.currPos.z * room.transform.GetComponent<Room>().Height * 10)
        );
        //방 실제 크기
        room.transform.localScale = new Vector3(
                    (room.transform.GetComponent<Room>().Width),
                     1,
                    (room.transform.GetComponent<Room>().Height));


        //기타 정보들. 위치, 이름, 타입 등등.
        room.transform.GetComponent<Room>().currPos = oldRoom.currPos;
        room.name = currentWorldName + "-" + oldRoom.roomName + " " + oldRoom.currPos.x + ", " + oldRoom.currPos.z;
        room.transform.GetComponent<Room>().roomName = oldRoom.roomName;
        room.transform.GetComponent<Room>().roomId = oldRoom.roomID;
        room.transform.GetComponent<Room>().isInRoute = oldRoom.isInRoute;
        room.transform.GetComponent<Room>().isParent = oldRoom.isLinked;
        room.transform.GetComponent<Room>().linked_num = oldRoom.room_linked_dir;

        //RoomController 오브젝트의 자식 객체로 설정
        room.transform.parent = transform;
        //리스트에 정보 부여
        loadedRooms.Add(room.GetComponent<Room>());
    }

    // 방 중복 여부
    public bool DoesRoomExist(float x, float y, float z)
    {
        return loadedRooms.Find(item => item.currPos.x == x && item.currPos.y == y && item.currPos.z == z) != null;
    }

    //조건에 맞는 방 찾음
    public Room FindRoom(float x, float y, float z)
    {
        // List.Find : item 변수 조건에 맞는 Room을 찾아 반환
        return loadedRooms.Find(item => item.currPos.x == x && item.currPos.y == y && item.currPos.z == z);
    }

    // 해당 Room에서 Player가 있는 방을 반환
    public void OnPlayerEnterRoom(Room room)
    {
        currRoom = room;

    }

}
