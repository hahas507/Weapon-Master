using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class RoomInfo
{
    public string roomID;
    public string roomName;
    public string roomType;
    public int room_linked_dir = 4; //0 : 왼쪽, 1 : 오른쪽, 2 : 위, 3 : 아래
    public int roomNum;

    // 현재 방의 위치
    public Vector3Int currPos;
    // 해당 방의 상태 설정(true : 방 셋팅, false : 빈방)
    public bool isInRoute;
    //
    public bool isLinked;


}
