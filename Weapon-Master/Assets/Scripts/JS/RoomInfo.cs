using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class RoomInfo
{
    public string roomID;
    public string roomName;
    public string roomType;
    public int room_linked_dir = 4; //0 : ����, 1 : ������, 2 : ��, 3 : �Ʒ�
    public int roomNum;

    // ���� ���� ��ġ
    public Vector3Int currPos;
    // �ش� ���� ���� ����(true : �� ����, false : ���)
    public bool isInRoute;
    //
    public bool isLinked;


}
