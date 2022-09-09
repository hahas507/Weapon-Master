using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    protected virtual void Awake() {}
    protected virtual void Start() {}
    protected virtual void Update() {}
    protected virtual void FixedUpdate() {}
    protected virtual void Exit() {}
}
