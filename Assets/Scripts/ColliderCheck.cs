using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public event EventHandler OnCarColliderEnter;
    public event EventHandler OnCarColliderStay;

    public static bool collisionFlag;

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
