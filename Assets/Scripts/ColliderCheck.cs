using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public event EventHandler OnCarColliderEnter;
    public event EventHandler OnCarColliderStay;

    public bool collisionFlag;

    public Transform parent { get; set; }

    private void Awake()
    {
        parent = transform.parent.transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionFlag = true;
        OnCarColliderEnter?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCarColliderStay?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionFlag = false;
    }
}
