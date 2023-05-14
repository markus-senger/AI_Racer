using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class DriveAgent : Agent
{
    [SerializeField] private TrackSubGoals trackSubGoals;
    [SerializeField] private ColliderCheck colliderCheck;
    [SerializeField] private CarController controller;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = controller.motorSphere.transform.position;
    }

    private void Start()
    {
        trackSubGoals.OnCarCorrectSubGoal += OnCarCorrectSubGoal;
        trackSubGoals.OnCarWrongSubGoal += OnCarWrongSubGoal;

        colliderCheck.OnCarColliderEnter += OnCarColliderEnter;
        colliderCheck.OnCarColliderStay += OnCarColliderStay;
    }

    private void OnCarColliderEnter(object sender, EventArgs e)
    {
        AddReward(-0.5f);
    }

    private void OnCarColliderStay(object sender, EventArgs e)
    {
        Debug.Log("END Ep");
        AddReward(-1.0f);
        EndEpisode();
    }

    private void OnCarCorrectSubGoal(object sender, EventArgs e)
    {
        AddReward(1f);
    }

    private void OnCarWrongSubGoal(object sender, EventArgs e)
    {
        AddReward(-1f);
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Start Ep");
        controller.ResetValues();
        trackSubGoals.ResetSubGoals();
        controller.motorSphere.transform.position = startPos;// + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, UnityEngine.Random.Range(-5f, 5f));
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actionSegment = actionsOut.ContinuousActions;
        actionSegment[0] = controller.GetInputMove();
        actionSegment[1] = controller.GetInputRotation();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 subGoalForward = trackSubGoals.GetNextSubGoal().transform.forward;
        float directionDot = Vector3.Dot(transform.forward, subGoalForward);
        sensor.AddObservation(directionDot);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Debug.Log("Move: " + Mathf.InverseLerp(0f, controller.fwdSpeed, actions.ContinuousActions[0]));
        //Debug.Log("Turn: " + Mathf.InverseLerp(-80f, 80f, actions.ContinuousActions[1]));
        /*controller.UpdateInputs(
            Mathf.InverseLerp(0f, controller.fwdSpeed, actions.ContinuousActions[0]),
            Mathf.InverseLerp(-80f, 80f, actions.ContinuousActions[1]));*/

        controller.UpdateInputs(
           actions.ContinuousActions[0],
           actions.ContinuousActions[1]);
    }
}
