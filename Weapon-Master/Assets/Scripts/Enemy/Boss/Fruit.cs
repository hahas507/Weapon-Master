using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private Vector3 playerPos;
    private Vector3 startPoint;
    private Vector3 landPoint;
    private Vector3 passPoint;
    private float t = 0;
    [SerializeField] [Range(0, 5)] private int speed;
    [SerializeField] [Range(0, 20)] private int height;
    [SerializeField] [Range(0, 5)] private int range;

    private void Awake()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void Start()
    {
        startPoint = transform.position;
        landPoint = playerPos + (new Vector3(Random.onUnitSphere.x, 0, Random.onUnitSphere.z)) * range;
        passPoint = landPoint + Vector3.up * height;
        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.position = new Vector3(
                       ThreePointBezier(startPoint.x, passPoint.x, landPoint.x),
                       ThreePointBezier(startPoint.y, passPoint.y, landPoint.y),
                       ThreePointBezier(startPoint.z, passPoint.z, landPoint.z));
            yield return null;
            if (t >= 1)
            {
                Destroy(gameObject, 3);
            }
        }
    }

    private float ThreePointBezier(float a, float b, float c)
    {
        return b + Mathf.Pow((1 - t), 2) * (a - b) + Mathf.Pow(t, 2) * (c - b);
    }
}