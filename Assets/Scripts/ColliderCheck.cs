using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public static bool collisionFlag;

    private void OnCollisionEnter(Collision collision)
    {
        collisionFlag = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionFlag = false;
    }
}
