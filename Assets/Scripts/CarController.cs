using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private RectTransform uiSpeedSlider;
    [SerializeField] private GameObject uiTurnSlider;

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
       
        uiSpeedSlider.sizeDelta = new Vector2(Utility.Map(moveSpeed, 0, fwdSpeed, 0, 200), 18);

        transform.position = motorSphere.transform.position;
    }

    private void HandleRotation()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Mouse X")) > mouseDeadZone)
            turnInput += Input.GetAxisRaw("Mouse X") * 3;

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
