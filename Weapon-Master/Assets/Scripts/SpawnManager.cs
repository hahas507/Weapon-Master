using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FunctionPointer(int param);

public class Spawn
{
    FunctionPointer spawnPattern;

    public Spawn(FunctionPointer pat)
    {
        this.spawnPattern = pat;
    }

    public void SpawnPattern(int idx)
    {
        spawnPattern(idx);
    }
}

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawnArea;

    List<GameObject> areas = new List<GameObject>(); //spawnAreas

    void Start()
    {
        InitAreas();
    }

    void InitAreas()
    {
        int cnt = spawnArea.transform.childCount;
        for (int i = 0; i < cnt; i++) areas.Add(spawnArea.transform.GetChild(i).gameObject);
    }

    void DestroyEnemy()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < objects.Length; i++) Destroy(objects[i]);
    }

    void SpawnEnemyCircle(int idx)
    {
        DestroyEnemy();
        Vector3 originPos = areas[idx].transform.position;

        for (int i = 0; i < 360; i += 36)
        {
            Vector3 pos = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), 0.0f, Mathf.Sin(i * Mathf.Deg2Rad));
            Vector3 spawnPos = pos * 10f + originPos;
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }

    void SpawnEnemyX(int idx)
    {
        DestroyEnemy();
        Vector3 originPos = areas[idx].transform.position;

        for (int i = 0, j = 0; i < 5; i++, j++)
        {
            Vector3 pos = new Vector3(-3.5f + 2.5f * i, 0.0f, -3.5f + 2.5f * i);
            Vector3 spawnPos = originPos + pos;
            Instantiate(enemy, spawnPos, Quaternion.identity);

            pos = new Vector3(-3.5f + 2.5f * i, 0.0f, 6.5f - 2.5f * i);
            spawnPos = originPos + pos;
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }
    }

    void SpawnEnemySquare(int idx)
    {
        DestroyEnemy();
        Vector3 originPos = areas[idx].transform.position;

        int cnt = 0, col = 0, z = 5;
        while (cnt < 9)
        {
            if (z == 5 && col > 2)
            {
                z = 0;
                col = 0;
            }
            if (z == 0 && col > 2)
            {
                z = -5;
                col = 0;
            }
            int x = -5 + 5 * col;

            Vector3 pos = new Vector3(x, 0, z);
            Vector3 spawnPos = originPos + pos;
            Instantiate(enemy, spawnPos, Quaternion.identity);

            ++col;
            ++cnt;
        }
    }

    public void SpawnEnemy()
    {
        FunctionPointer[] pattern = new FunctionPointer[] { SpawnEnemyCircle, SpawnEnemyX, SpawnEnemySquare };
        int areaIdx = Random.Range(0, areas.Count);
        int patIdx = Random.Range(0, pattern.Length);

        Spawn spawn = new Spawn(pattern[patIdx]);
        spawn.SpawnPattern(areaIdx);
    }
}