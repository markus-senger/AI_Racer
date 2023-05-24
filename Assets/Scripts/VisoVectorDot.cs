using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VisoVectorDot : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float vectorLength = 1f;

    [SerializeField] private float rotationSpeed;

    private float currentRotation;

    private void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        currentRotation %= 360f;
        target.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
    }

    private void OnDrawGizmos()
    {
        Vector3 endPoint = transform.position + transform.forward * vectorLength;
        Handles.color = Color.blue;
        Handles.DrawAAPolyLine(10, transform.position, endPoint);

        float dotProduct = Vector3.Dot(transform.forward, target.transform.forward);
        Handles.color = Color.red;
        Handles.DrawAAPolyLine(10, transform.position, transform.position + transform.forward * dotProduct * 2);

        endPoint = transform.position + target.transform.forward * vectorLength;
        Handles.color = Color.green;
        Handles.DrawAAPolyLine(10, transform.position, endPoint);
    }
}
