using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private bool isTreeDown = false;

    public bool ISTREEDOWN
    {
        get { return isTreeDown; }
        private set { isTreeDown = value; }
    }

    //private void Awake()
    //{
    //    isTreeDown = false;
    //}
}