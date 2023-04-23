using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Rigidbody motorSphere;
    [SerializeField] private float fwdSpeed;
    [SerializeField] private float revSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float turnRadius;
    [SerializeField] private float mouseDeadZone;

    [SerializeField] private GameObject brakeLights;
    [SerializeField] private GameObject reversingLights;

    private float moveInput;
    private float oldMoveInput;
    private float moveSpeed;
    private float turnInput;

    private int cntForBrakeLightDuration;
    private int brakeLightDuration = 10;

    private void Start()
    {
        motorSphere.transform.parent = null;
        ResetValues();
    }

    private void Update()
    {
        oldMoveInput = moveInput;
        moveInput += Input.GetAxisRaw("Mouse ScrollWheel") / 3;

        HandleBrakeLights();

        if (moveInput <= 0)
            ResetValues();

        HandleRotation();
        HandleMoveSpeed();
    }

    private void HandleMoveSpeed()
    {
        moveSpeed = moveInput * (moveInput > 0 ? fwdSpeed : revSpeed);

        if (moveSpeed > fwdSpeed)
        {
            moveSpeed = fwdSpeed;
            moveInput = oldMoveInput;
        }

        transform.position = motorSphere.transform.position;
    }

    private void HandleRotation()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Mouse X")) > mouseDeadZone)
            turnInput += Input.GetAxisRaw("Mouse X") * 3;

        if (Mathf.Abs(turnInput) > turnRadius)
            turnInput = turnInput > 0 ? turnRadius : -turnRadius;

        if (moveSpeed > 0)
        {
            float newRotation = turnInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0, newRotation, 0, Space.World);
        }
    }

    private void HandleBrakeLights()
    {
        if (oldMoveInput > moveInput) brakeLights.SetActive(true);
        else if (cntForBrakeLightDuration > brakeLightDuration)
        {
            brakeLights.SetActive(false);
            cntForBrakeLightDuration = 0;
        }
        else cntForBrakeLightDuration++;
    }

    private void FixedUpdate()
    {
        motorSphere.AddForce(transform.forward * moveSpeed, ForceMode.Acceleration);
    }

    private void ResetValues()
    {
        moveSpeed = 0;
        moveInput = 0;
        turnInput = 0;
    }
}
