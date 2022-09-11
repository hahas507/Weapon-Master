using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonCreator : Singleton<DungeonCreator>
{

    //방 개수 및 최대거리
    public int currRoomCnt = 0;
    public int maxDistance;
    public int row_num;
    public int col_num;
    public int boss_num;


    public Vector3Int startPoint = new Vector3Int(0, 0, 0);

    public RoomInfo[,] posArr = new RoomInfo[10, 10];



    public void GetBossnum(int boss_num_fnc)
    {

        boss_num = boss_num_fnc;
    }

public void DungeonCreate()
    {
        RealaseRoom();
        //방 배열 생성
        for (int i = 0; i < col_num; i++)
        {
            for (int j = 0; j < row_num; j++)
            {
                if (i == (col_num - 1) && j == boss_num)
                {
                    posArr[j, i] = BossRoom(new RoomInfo(), new Vector3Int(i, 0, j), "Boss");
                }
                else
                {
                    posArr[j, i] = SingleRoom(new RoomInfo(), new Vector3Int(i, 0, j), "Single");
                }

            }
        }
        MakeRoute(posArr);
        MakeParentRoom(posArr);
        setupPosition();

    }

    //시작지점에서 보스룸까지 주 경로 생성
    public RoomInfo[,] MakeRoute(RoomInfo[,] posArr)
    {
        int row = 0;
        int col = 0;
        int count = 0;
        while (row <=row_num && col <= col_num)
        {
            // 0:위, 1:오른쪽
            int direction = Random.Range(0, 2);


            if (direction == 0) //위
            {
                if ((row + 1) == row_num) //위에 방 없을 때
                {
                    if ((col + 1) == col_num) //우측에도 방 없을 때. 아래를 경로에 추가
                    {
                        while (posArr[row, col].roomName != "Boss") //쭉 아래로
                        {
                            posArr[row - 1, col].isInRoute = true;
                            row--;
                            count++;
                        }
                    }
                    else //우측을 경로에 추가
                    {
                        posArr[row, col + 1].isInRoute = true;
                        col++;
                        count++;
                    }
                }
                else//위를 경로에 추가
                {
                    posArr[row + 1, col].isInRoute = true;
                    row++;
                    count++;
                }
            }
            else if (direction == 1) //오른쪽
            {
                if ((col + 1) == col_num) //우측에 방 없을 때
                {
                    if ((row + 1) == row_num) //위에도 방 없을 때. 아래를 경로에 추가
                    {
                        while (posArr[row, col].roomName != "Boss")
                        {
                            posArr[row - 1, col].isInRoute = true;
                            row--;
                            count++;
                        }
                    }
                    else //위를 경로로 추가
                    {
                        posArr[row + 1, col].isInRoute = true;
                        row++;
                        count++;
                    }
                }
                else
                {
                    posArr[row, col + 1].isInRoute = true;
                    col++;
                    count++;
                }
            }
            currRoomCnt++;

            //보스룸에 도달하면 반복문 종료
            if (posArr[row, col].roomName == "Boss")
            {
                break;
            }
            //오류 체크
            else if (count >= 30)
            {
                break;
            }
        }

        return posArr;
    }


    //주경로에서 뻗어나오는 막다른방
    public RoomInfo[,] MakeParentRoom(RoomInfo[,] posArr)
    {
        int parent_count = 0;
        //모든 방 중 생성안된 방 검사
        for (int i = 0; i < col_num; i++)
        {
            for (int j = 0; j < row_num; j++)
            {
                //무작위로 방향 지정 및 방 활성화
                while (!posArr[j, i].isInRoute)
                {
                    int door_dir =  Random.Range(0, 4);
                    posArr[j, i].room_linked_dir = door_dir;
                    switch (door_dir)
                    {
                        //왼쪽
                        case 0:
                            if (!(i == 0) && posArr[j, i - 1].isInRoute && posArr[j, i - 1].roomName == "Single")
                            {
                                posArr[j, i].isInRoute = true;
                                parent_count++;
                            }
                            break;
                        //오른쪽
                        case 1:
                            if (!(i == col_num - 1) && posArr[j, i + 1].isInRoute && posArr[j, i + 1].roomName == "Single")
                            {
                                posArr[j, i].isInRoute = true;
                                parent_count++;

                            }
                            break;
                        //위
                        case 2:
                            if (!(j == row_num - 1) && posArr[j + 1, i].isInRoute && posArr[j + 1, i].roomName == "Single")
                            {
                                posArr[j, i].isInRoute = true;
                                parent_count++;
                            }
                            break;
                        //아래
                        case 3:
                            if (!(j == 0) && posArr[j - 1, i].isInRoute && posArr[j - 1, i].roomName == "Single")
                            {
                                posArr[j, i].isInRoute = true;
                                parent_count++;
                            }
                            break;
                    }

                }

                if (parent_count == 1)
                {
                    j = row_num;
                }
  
            }
        }

        return posArr;
    }

    // 모든 변수를 초기화
    public void RealaseRoom()
    {
        roomPosInit();
        currRoomCnt = 0;
    }


    public void roomPosInit()
    {
        int boss_num_fnc;
        if (row_num > 3)
        {
            boss_num_fnc = Random.Range(1, row_num);
        }
        else
        {
            boss_num_fnc = Random.Range(row_num - 2, row_num);
        }
        GetBossnum(boss_num_fnc);
        if (col_num > 5 || row_num > 5)
        {
            Debug.Log("최대 가능한 행/열 수는 5이하!");
        }
        for (int i = 0; i < col_num; i++)
        {
            for (int j = 0; j < row_num; j++)
            {
                
                posArr[j, i] = new RoomInfo();
                posArr[j, i].isInRoute = false;
            }
        }
    }

    // 배열의 방들을 RoomController의 List로 변환
    public void setupPosition()
    {
        List<RoomInfo> positions = new List<RoomInfo>();

        for (int i = 0; i < col_num; i++)
        {
            for (int j = 0; j < row_num; j++)
            {
                if (posArr[j, i].isInRoute)
                {
                    Vector3Int tmp = new Vector3Int(i, 0, j);


                    if (i == (col_num - 1) && j == boss_num)
                    {
                        posArr[j, i] = BossRoom(posArr[j, i], posArr[j, i].roomName);
                    }
                    else
                    {
                        posArr[j, i] = SingleRoom(posArr[j, i], posArr[j, i].roomName);
                    }
                    positions.Add(posArr[j, i]);
                }
            }
        }

        //반복문을 통해 모든 생성된 방을 RoomController에 전달
        for (int i = 0; i < positions.Count; i++)
            RoomManager.Instance.LoadRoom(positions[i]);

    }

    public RoomInfo BossRoom(RoomInfo room, Vector3Int pos, string name)
    {
        RoomInfo boss = room;
        boss.roomID = name + "(" + pos.x + ", " + pos.y + ", " + pos.z + ")";
        boss.roomName = name;
        boss.currPos = pos;
        boss.isInRoute = false;
        return boss;
    }

    public RoomInfo BossRoom(RoomInfo pos, string name)
    {
        RoomInfo boss = pos;
        boss.roomID = name + "(" + pos.currPos.x + ", " + pos.currPos.y + ", " + pos.currPos.z + ")";
        boss.roomName = name;
        boss.currPos = pos.currPos;
        boss.isInRoute = pos.isInRoute;
        boss.isLinked = pos.isLinked;
        return boss;
    }

    public RoomInfo SingleRoom(RoomInfo room, Vector3Int pos, string name)
    {
        RoomInfo single = room;
        single.roomID = name + "(" + pos.x + ", " + pos.y + ", " + pos.z + ")";
        single.roomName = name;
        single.currPos = pos;

        if (pos == new Vector3Int(0, 0, 0))
        {
            single.isInRoute = true;

        }
        else
        {
            single.isInRoute = false;
        }
        return single;
    }

    public RoomInfo SingleRoom(RoomInfo pos, string name)
    {
        RoomInfo single = pos;
        single.roomID = name + "(" + pos.currPos.x + ", " + pos.currPos.y + ", " + pos.currPos.z + ")";
        single.roomName = name;
        single.currPos = pos.currPos;
        single.isInRoute = pos.isInRoute;
        single.isLinked = pos.isLinked;
        return single;
    }

}
