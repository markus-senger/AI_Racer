using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGoal : MonoBehaviour
{
    private TrackSubGoals trackSubGoals;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ColliderCheck>(out ColliderCheck collider))
        {
            trackSubGoals.CarThroughSubGoal(this, collider.parent);
        }
    }

    public void SetTrackSubGoal(TrackSubGoals trackSubGoals)
    {
        this.trackSubGoals = trackSubGoals;
    }
}
