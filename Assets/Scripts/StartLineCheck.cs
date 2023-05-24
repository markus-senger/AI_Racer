using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartLineCheck : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text bestTimeText;

    public event EventHandler OnNewRound;

    private bool isActive;
    private bool newRound;
    private float timeValue;
    private float milliseconds;
    private float bestTimeValue;
    public static bool triggerActive { get;  set; }

    private void Start()
    {
        bestTimeValue = Mathf.Infinity;
        triggerActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerActive)
        {
            if (isActive)
            {
                newRound = true;
                OnNewRound?.Invoke(this, EventArgs.Empty);
            }
            isActive = true;
            triggerActive = false;
        }
    }

    private void Update()
    {
        if(isActive)
        {
            if (newRound && bestTimeValue > timeValue)
            {
                bestTimeValue = timeValue;
                bestTimeText.text = string.Format("Best: {0:00}:{1:00}:{2:000}",
                           Mathf.FloorToInt(milliseconds / 60000),
                           Mathf.FloorToInt((milliseconds / 1000) % 60),
                           Mathf.FloorToInt(milliseconds % 1000));
            }
            if(newRound)
            {
                timeValue = 0;
                newRound = false;
            }
            timeValue += Time.deltaTime;
            milliseconds = timeValue * 1000f;
            timeText.text = string.Format("Time: {0:00}:{1:00}:{2:000}",
                           Mathf.FloorToInt(milliseconds / 60000),
                           Mathf.FloorToInt((milliseconds / 1000) % 60),
                           Mathf.FloorToInt(milliseconds % 1000));

        }
    }
}
