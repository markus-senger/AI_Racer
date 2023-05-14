using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGoal : MonoBehaviour
{
    private TrackSubGoals trackSubGoals;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<SphereCollider>(out SphereCollider sphereCollider))
        {
            trackSubGoals.CarThroughSubGoal(this);
        }
    }

    public void SetTrackSubGoal(TrackSubGoals trackSubGoals)
    {
        this.trackSubGoals = trackSubGoals;
    }
}
