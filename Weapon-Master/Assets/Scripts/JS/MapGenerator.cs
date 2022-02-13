using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;
    //바닥타일 범위
    [Range(0, 1)]
    public float outlinePercent;
    //장애물 비중
    [Range(0, 1)]
    public float obstaclePercent;

    List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    //랜덤 시드, 딱히 상관없음
    public int seed = 10;

    //맵 중앙 좌표. 일단 플레이어 스폰 지점
    Coord mapCenter;


    void Start()
    {
        GenerateMap();
    }


    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));
        mapCenter = new Coord((int)mapSize.x/2, (int)mapSize.y/2);
        
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
            // 에디터에 의해 호출되기 때문에 Destroy 대신 DestroyImmediate사용
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        //타일생성
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right*90)) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent);
                newTile.parent = mapHolder;//타일 파괴
            }
        }

        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

        //장애물생성
        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent); //장애물 개수 판단 : 전체맵에서 장애물 비중
        int currentObstacleCount = 0;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;
            if (randomCoord != mapCenter && MapIsFullyAccessible(obstacleMap, currentObstacleCount)) //플레이어 스폰위치가 아닐때만 장애물 생성
            {
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);

                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder; //장애물파괴
            }
            else //생성못함
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }

    }

    //장애물 좌표 중복여부
    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount) //중앙에서 시작해 장애물 개수 감지, Flood Fill알고리즘
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCenter);
        mapFlags[mapCenter.x, mapCenter.y] = true;

        int accessibleTileCount = 1; //접근가능한 타일 수


        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            //중앙과 이웃한 타일 파악
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1)) //검사하지 않은 이웃타일
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY]) //장애물 타일이 아니라면
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Coord(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTile = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTile == accessibleTileCount; //목표로 한 타일과 같은지
    }

    //위치로 변환
    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

		

    public Coord GetRandomCoord()
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }


    public struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        //재정립(오류수정위함)
        public static bool operator ==(Coord c1, Coord c2)
        {
            return c1.x == c2.x && c1.y == c2.y;
        }
        public static bool operator !=(Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }


    }

}
