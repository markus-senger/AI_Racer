using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform racingCar;
    [SerializeField] private Vector3 offsetLocation;
    [SerializeField] private Vector3 offsetRotation;
    [SerializeField] private float smooth;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = racingCar.position + racingCar.rotation * offsetLocation;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smooth);
        transform.position = smoothedPosition;

        Quaternion desiredrotation = racingCar.rotation * Quaternion.Euler(offsetRotation);
        Quaternion smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, smooth);
        transform.rotation = smoothedrotation;
    }
}
