using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubRoom : MonoBehaviour
{

    public int Width;
    public int Height;

    public string roomName;
    public string roomType;

    // �� ���� ���� ����
    public List<Door> doors;
    public Door leftDoor;
    public Door rightDoor;
    public Door topDoor;
    public Door bottomDoor;

    public List<Wall> walls;
    public Wall leftWall;
    public Wall rightWall;
    public Wall topWall;
    public Wall bottomWall;

    // ���� �� ��ġ
    public Vector3 currPos;
    public Vector3 parentPos;
    public Vector3 CenterPos;
    public string wallType;

    public bool updatedRooms = false;
    public bool roomPathBool = false;
    public bool isParent;
    public int linked_num; //0 : ����, 1 : ������, 2 : ��, 3 : �Ʒ�\

    public RoomInfo roomInfo;

    void Start()
    {
        Door[] ds = GetComponentsInChildren<Door>();

        foreach (Door d in ds)
        {
            // doors ����Ʈ�� Door�� ����(
            doors.Add(d);

            switch (d.doorType)
            {
                case Door.DoorType.right:
                    rightDoor = d;
                    break;
                case Door.DoorType.left:
                    leftDoor = d;
                    break;
                case Door.DoorType.top:
                    topDoor = d;
                    break;
                case Door.DoorType.bottom:
                    bottomDoor = d;
                    break;
            }
        }

        Wall[] ws = GetComponentsInChildren<Wall>();

        foreach (Wall w in ws)
        {
            // walls ����Ʈ�� Wall�� ����
            walls.Add(w);

            switch (w.wallType)
            {
                case Wall.WallType.left:
                    leftWall = w;
                    break;
                case Wall.WallType.top:
                    topWall = w;
                    break;
                case Wall.WallType.right:
                    rightWall = w;
                    break;
                case Wall.WallType.bottom:
                    bottomWall = w;
                    break;
            }
        }


    }

    private void Update()
    {
        RoomUpdate();
    }

    void RoomUpdate()
    {
        if (!updatedRooms)
        {
            CreateDoors();

            updatedRooms = true;
        }

    }

    //�ֺ� �� ���� �ľ� �� �� ����
    public void CreateDoors()
    {
        string wallStr = "";

        foreach (Wall wall in walls)
        {
            switch (wall.wallType)
            {
                //���� 0
                case Wall.WallType.left:
                    if (GetLeft() != null
                        && (linked_num == 4 || linked_num == 0)
                        && (GetLeft().linked_num == 4 || GetLeft().linked_num == 1))
                    {
                        Room leftRoom = GetLeft();
                        wallStr += "L";
                        if (!leftDoor.isUpdate)
                        {
                            GameObject roomDoor = Instantiate(leftRoom.Door, leftDoor.transform);
                            roomDoor.gameObject.transform.SetParent(leftDoor.gameObject.transform);
                            leftDoor.setNextRoom(leftRoom.gameObject);
                            leftDoor.SideDoor = leftRoom.rooms.rightDoor;

                            leftDoor.isUpdate = true;
                        }
                    }
                    else
                    {
                        leftDoor.gameObject.SetActive(false);
                    }
                    break;
                //������ 1
                case Wall.WallType.right:
                    if (GetRight() != null
                        && (linked_num == 4 || linked_num == 1)
                        && (GetRight().linked_num == 4 || GetRight().linked_num == 0))
                    {
                        Room rightRoom = GetRight();
                        wallStr += "R";
                        if (!rightDoor.isUpdate)
                        {
                            GameObject roomDoor = Instantiate(rightRoom.Door, rightDoor.transform);
                            roomDoor.gameObject.transform.SetParent(rightDoor.gameObject.transform);

                            rightDoor.setNextRoom(rightRoom.gameObject);
                            rightDoor.SideDoor = rightRoom.rooms.leftDoor;

                            rightDoor.isUpdate = true;
                        }
                    }
                    else
                    {
                        rightDoor.gameObject.SetActive(false);
                    }
                    break;
                //�� 2
                case Wall.WallType.top:

                    if (GetTop() != null
                        && (linked_num == 4 || linked_num == 2)
                        && (GetTop().linked_num == 4 || GetTop().linked_num == 3))
                    {
                        Room topRoom = GetTop();
                        wallStr += "T";
                        if (!topDoor.isUpdate)
                        {
                            GameObject roomDoor = Instantiate(topRoom.Door, topDoor.transform);
                            roomDoor.gameObject.transform.SetParent(topDoor.gameObject.transform);
                            topDoor.setNextRoom(topRoom.gameObject);
                            topDoor.SideDoor = topRoom.rooms.bottomDoor;

                            topDoor.isUpdate = true;
                        }
                    }
                    else
                    {
                        topDoor.gameObject.SetActive(false);
                    }
                    break;

                //�Ʒ� 3
                case Wall.WallType.bottom:
                    if (GetBottom() != null
                        && (linked_num == 4 || linked_num == 3)
                        && (GetBottom().linked_num == 4 || GetBottom().linked_num == 2))
                    {
                        Room bottomRoom = GetBottom();

                        wallStr += "B";
                        if (!bottomDoor.isUpdate)
                        {
                            GameObject roomDoor = Instantiate(bottomRoom.Door, bottomDoor.transform);
                            roomDoor.gameObject.transform.SetParent(bottomDoor.gameObject.transform);

                            bottomDoor.setNextRoom(bottomRoom.gameObject);
                            bottomDoor.SideDoor = bottomRoom.rooms.topDoor;

                            bottomDoor.isUpdate = true;
                        }
                    }
                    else
                    {
                        bottomDoor.gameObject.SetActive(false);
                    }
                    break;

            }

        }

        if (wallStr != "")
            wallType = wallStr;
        else
        {
            wallType = "None";
            this.gameObject.SetActive(false);
        }

    }
    //�����ʿ� ���� �ִ°�
    public Room GetRight()
    {
        if (RoomManager.Instance.DoesRoomExist(currPos.x + 1, currPos.y, currPos.z))
        {
            return RoomManager.Instance.FindRoom(currPos.x + 1, currPos.y, currPos.z);
        }
        return null;
    }

    //���ʿ� ���� �ִ°�
    public Room GetLeft()
    {
        if (RoomManager.Instance.DoesRoomExist(currPos.x - 1, currPos.y, currPos.z))
        {
            return RoomManager.Instance.FindRoom(currPos.x - 1, currPos.y, currPos.z);
        }
        return null;
    }

    //���ʿ� ���� �ִ°�
    public Room GetTop()
    {
        if (RoomManager.Instance.DoesRoomExist(currPos.x, currPos.y, currPos.z + 1))
        {
            return RoomManager.Instance.FindRoom(currPos.x, currPos.y, currPos.z + 1);
        }
        return null;
    }

    

    //�Ʒ��ʿ� ���� �ִ°�
    public Room GetBottom()
    {
        if (RoomManager.Instance.DoesRoomExist(currPos.x, currPos.y, currPos.z - 1))
        {
            return RoomManager.Instance.FindRoom(currPos.x, currPos.y, currPos.z - 1);
        }
        return null;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            RoomManager.Instance.OnPlayerEnterRoom(this.transform.parent.GetComponent<Room>());
        }
    }


}
