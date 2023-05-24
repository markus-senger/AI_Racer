using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSubGoals : MonoBehaviour
{
    public event EventHandler OnCarCorrectSubGoal;
    public event EventHandler OnCarWrongSubGoal;

    private List<Transform> carTransformList;
    private List<SubGoal> subGoalList;
    private List<int> nextSubGoalIndexList;

    private void Awake()
    {
        Transform subGoalsTransform = transform.Find("SubGoals");
        subGoalList = new List<SubGoal>();
        nextSubGoalIndexList = new List<int>();
        carTransformList = new List<Transform>();

        var goArray = FindObjectsOfType<CarController>();
        for (var i = 0; i < goArray.Length; i++)
        {
            carTransformList.Add(goArray[i].gameObject.transform);
            nextSubGoalIndexList.Add(0);
        }

        foreach (Transform subGoalTransform in subGoalsTransform)
        {
            SubGoal subGoal = subGoalTransform.GetComponent<SubGoal>();
            subGoal.SetTrackSubGoal(this);
            subGoalList.Add(subGoal);
        }

    }

    public int GetListSize()
    {
        return subGoalList.Count;
    }

    public int GetNextIndex(Transform carTransform)
    {
        return nextSubGoalIndexList[carTransformList.IndexOf(carTransform)];
    }

    public void ResetSubGoals(Transform transform)
    {
        nextSubGoalIndexList[carTransformList.IndexOf(transform)] = 0;
    }

    public SubGoal GetNextSubGoal(Transform carTransform)
    {
        return subGoalList[nextSubGoalIndexList[carTransformList.IndexOf(carTransform)]];
    }

    public void CarThroughSubGoal(SubGoal subGoal, Transform carTransform)
    {
        int nextSubGoalIndex = nextSubGoalIndexList[carTransformList.IndexOf(carTransform)];

        if (subGoalList.IndexOf(subGoal) == nextSubGoalIndex)
        {
            nextSubGoalIndexList[carTransformList.IndexOf(carTransform)] = (nextSubGoalIndex + 1) % subGoalList.Count;
            OnCarCorrectSubGoal?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnCarWrongSubGoal?.Invoke(this, EventArgs.Empty);
        }
    }
}
