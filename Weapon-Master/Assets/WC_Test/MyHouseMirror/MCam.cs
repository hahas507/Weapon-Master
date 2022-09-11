using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCam : MonoBehaviour
{
    Camera MirrorCam;
    Vector3 MirrorScale;
    void Start(){
        MirrorScale = MirrorCam.transform.parent.localScale;
        MirrorCam = GetComponent<Camera>();
        
    }
}
