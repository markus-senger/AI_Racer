using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Policies;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public Rigidbody motorSphere;
    [SerializeField] private Rigidbody car;

    public float fwdSpeed;
    [SerializeField] private float revSpeed;
    [SerializeField] private float collisionImpact;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float turnRadius;
    [SerializeField] private float mouseDeadZone;

    [SerializeField] private GameObject brakeLights;
    [SerializeField] private GameObject reversingLights;
    [SerializeField] private TrailRenderer[] trails;

    [SerializeField] private RectTransform uiSpeedSlider;
    [SerializeField] private GameObject uiTurnSlider;

    [SerializeField] private bool driveManually;

    private float moveInput;
    private float oldMoveInput;
    public float moveSpeed;
    public float turnInput;

    private int cntForBrakeLightDuration;
    private int brakeLightDuration = 10;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        motorSphere.transform.parent = null;
        car.transform.parent = null;
        ResetValues();
    }

    private void Update()
    {
        oldMoveInput = moveInput;
        if (driveManually)
            moveInput += GetInputMove();

        HandleBrakeLights();

        car.MoveRotation(transform.rotation);

        if (moveInput <= 0)
            ResetValues();

        HandleRotation();
        HandleMoveSpeed();
        HandleTrails();
    }

    public void ResetValues()
    {
        moveSpeed = 0;
        moveInput = 0;
        turnInput = 0;
    }

    public void UpdateInputs(float moveInput, float turnInput)
    {
        this.moveInput += moveInput;
        this.turnInput += turnInput;
    }

    public float GetInputMove()
    {
        return Input.GetAxisRaw("Mouse ScrollWheel") / 3;
    }

    public float GetInputRotation()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Mouse X")) > mouseDeadZone)
            return Input.GetAxisRaw("Mouse X") * 3;

        return 0;
    }

    private void HandleMoveSpeed()
    {
        moveSpeed = moveInput * (moveInput > 0 ? fwdSpeed : revSpeed);

        if (moveSpeed > fwdSpeed)
        {
            moveSpeed = fwdSpeed;
            moveInput = oldMoveInput;
        }

        uiSpeedSlider.sizeDelta = new Vector2(Utility.Map(moveSpeed, 0, fwdSpeed, 0, 200), 18);

        transform.position = motorSphere.transform.position;
    }

    private void HandleRotation()
    {
        if(driveManually)
            turnInput += GetInputRotation();

        if (Mathf.Abs(turnInput) > turnRadius)
            turnInput = turnInput > 0 ? turnRadius : -turnRadius;

        float newRotation = 0;
        if (moveSpeed > 0)
        {
            newRotation = turnInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0, newRotation, 0, Space.World);

            uiTurnSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(Utility.Map(Mathf.Abs(turnInput), 0, turnRadius, 0, 200), 18);
            if (turnInput > 0) uiTurnSlider.GetComponent<Image>().color = new Color(0.6313726f, 0.6982701f, 0.9741356f);
            else uiTurnSlider.GetComponent<Image>().color = new Color(0.6313726f, 0.9921569f, 0.9741356f);
        }
        else
            uiTurnSlider.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 18);
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

    private void HandleTrails()
    {
        if(Mathf.Abs(turnInput) > turnRadius / 2 && moveSpeed > fwdSpeed / 5)
        {
            foreach(var trail in trails)
            {
                trail.emitting = true;
            }
        }
        else
        {
            foreach (var trail in trails)
            {
                trail.emitting = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (car.GetComponent<ColliderCheck>().collisionFlag)
        {
            moveInput -= collisionImpact;
            if (moveInput < 0) moveInput = 0.1f;
            car.GetComponent<ColliderCheck>().collisionFlag = false;
        }

        motorSphere.AddForce(transform.forward * moveSpeed, ForceMode.Acceleration);       
    }
}
