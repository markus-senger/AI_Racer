using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float steeringInput;
    private float accelerationInput;
    private bool isBreakingInput;

    [SerializeField] private float accelerationForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private GameObject brakeLights;
    [SerializeField] private GameObject reversingLights;

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleCarAcceleration();
        HandleBreaking();
        HandleCarSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        steeringInput = Input.GetAxis("Horizontal");
        accelerationInput = Input.GetAxis("Vertical");
        isBreakingInput = Input.GetKey(KeyCode.Space);
    }

    private void HandleCarAcceleration()
    {
        float torque = accelerationInput * accelerationForce;
        frontLeftWheelCollider.motorTorque = torque;
        frontRightWheelCollider.motorTorque = torque;

        if(torque < 0) reversingLights.SetActive(true);
        else reversingLights.SetActive(false);
    }

    private void HandleBreaking()
    {
        if(isBreakingInput) brakeLights.SetActive(true);
        else brakeLights.SetActive(false);

        frontRightWheelCollider.brakeTorque = isBreakingInput ? breakForce : 0; 
        frontLeftWheelCollider.brakeTorque = isBreakingInput ? breakForce : 0; 
        rearLeftWheelCollider.brakeTorque = isBreakingInput ? breakForce : 0; 
        rearRightWheelCollider.brakeTorque = isBreakingInput ? breakForce : 0; 
    }

    private void HandleCarSteering()
    {
        frontLeftWheelCollider.steerAngle = maxSteerAngle * steeringInput;  // steeringInput is normalized (-1 to 1)
        frontRightWheelCollider.steerAngle = maxSteerAngle * steeringInput;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
