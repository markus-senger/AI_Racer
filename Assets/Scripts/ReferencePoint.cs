using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StartLineCheck.triggerActive = true;
    }
}
