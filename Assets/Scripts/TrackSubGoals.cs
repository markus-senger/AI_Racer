using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSubGoals : MonoBehaviour
{
    public event EventHandler OnCarCorrectSubGoal;
    public event EventHandler OnCarWrongSubGoal;

    private List<SubGoal> subGoalList;
    private int nextSubGoalIndex;

    private void Awake()
    {
        Transform subGoalsTransform = transform.Find("SubGoals");
        subGoalList = new List<SubGoal>();
        nextSubGoalIndex = 0;

        foreach (Transform subGoalTransform in subGoalsTransform)
        {
            SubGoal subGoal = subGoalTransform.GetComponent<SubGoal>();
            subGoal.SetTrackSubGoal(this);
            subGoalList.Add(subGoal);
        }

    }

    public void ResetSubGoals()
    {
        nextSubGoalIndex = 0;
    }

    public SubGoal GetNextSubGoal()
    {
        return subGoalList[nextSubGoalIndex];
    }

    public void CarThroughSubGoal(SubGoal subGoal)
    {
        if(subGoalList.IndexOf(subGoal) == nextSubGoalIndex)
        {
            nextSubGoalIndex = (nextSubGoalIndex + 1) % subGoalList.Count;
            OnCarCorrectSubGoal?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnCarWrongSubGoal?.Invoke(this, EventArgs.Empty);
        }
    }
}
