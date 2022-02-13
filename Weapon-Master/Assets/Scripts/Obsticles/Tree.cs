using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Status
{
    private Animator anim;
    [SerializeField] private bool isTreeDown = false;

    public bool ISTREEDOWN
    {
        get { return isTreeDown; }
        private set { isTreeDown = value; }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (HP <= 0)
        {
            isTreeDown = true;
            anim.SetTrigger("isDown");
        }
    }
}