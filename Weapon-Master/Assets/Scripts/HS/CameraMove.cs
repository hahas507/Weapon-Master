using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float nonTargetingDist;
    public float nonTargetingHeight;
    public float targetingDist;
    public float targetingHeight;
    public float rotateSpeed;

    float dist, height;

    void Start()
    {
        dist = nonTargetingDist;
        height = nonTargetingHeight;
    }

    void Update()
    {
        float currY = Mathf.LerpAngle(this.transform.eulerAngles.y, target.eulerAngles.y, rotateSpeed * Time.deltaTime);
        Quaternion rot = Quaternion.Euler(0, currY, 0);

        if (PlayerController.targetingMode)
        {
            if (dist != targetingDist && height != targetingHeight) //zoom in
            {
                dist = Mathf.Lerp(dist, targetingDist, rotateSpeed * Time.deltaTime);
                height = Mathf.Lerp(height, targetingHeight, rotateSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (dist != nonTargetingDist && height != nonTargetingHeight) //zoom out
            {
                dist = Mathf.Lerp(dist, nonTargetingDist, rotateSpeed * Time.deltaTime);
                height = Mathf.Lerp(height, nonTargetingHeight, rotateSpeed * Time.deltaTime);
            }
        }

        this.transform.position = target.position - (rot * Vector3.forward * dist) + (Vector3.up * height);
        this.transform.LookAt(target);
    }
}