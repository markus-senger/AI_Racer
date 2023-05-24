using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;
using Unity.MLAgents.Actuators;

public class DriveAgent : Agent
{
    [SerializeField] private TrackSubGoals trackSubGoals;
    [SerializeField] private StartLineCheck startLineCheck;
    [SerializeField] private ColliderCheck colliderCheck;
    [SerializeField] private CarController controller;
    [SerializeField] private SphereCollider motorSphere;

    private Vector3 startPosSphere;

    private void Awake()
    {
        startPosSphere = controller.gameObject.transform.position;
    }

    private void Start()
    {
        trackSubGoals.OnCarCorrectSubGoal += OnCarCorrectSubGoal;
        trackSubGoals.OnCarWrongSubGoal += OnCarWrongSubGoal;

        colliderCheck.OnCarColliderEnter += OnCarColliderEnter;
    }

    private void OnCarColliderEnter(object sender, EventArgs e)
    {
        Debug.Log("End Round - ColliderEnter");
        AddReward(-2f);
        EndEpisode();
    }

    private void OnCarCorrectSubGoal(object sender, EventArgs e)
    {
        Debug.Log("CorrectSubgoal: 1");
        AddReward(1f);
        if (trackSubGoals.GetNextIndex(controller.gameObject.transform) == 0)
        {
            Debug.Log("Fin Round");
            trackSubGoals.ResetSubGoals(controller.gameObject.transform);
            AddReward(1f);
            //EndEpisode();
        }
    }

    private void OnCarWrongSubGoal(object sender, EventArgs e)
    {
        Debug.Log("WrongSubgoal: -1f");
        AddReward(-1f);
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Start Ep");

        colliderCheck.GetComponent<BoxCollider>().enabled = false;
        motorSphere.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        controller.ResetValues();
        trackSubGoals.ResetSubGoals(controller.gameObject.transform);
        motorSphere.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        motorSphere.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        controller.transform.rotation = Quaternion.Euler(0, 0, 0);
        motorSphere.gameObject.transform.position = startPosSphere + new Vector3(UnityEngine.Random.Range(-3f, 3f), 0, UnityEngine.Random.Range(-1f, 1f));
        transform.rotation = Quaternion.Euler(0,0,0);
        StartCoroutine(RespawnDecisionRequester(0.1f));
    }

    private IEnumerator RespawnDecisionRequester(float delay)
    {
        yield return new WaitForSeconds(delay);

        motorSphere.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        colliderCheck.GetComponent<BoxCollider>().enabled = true;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> actionSegment = actionsOut.DiscreteActions;
        actionSegment[0] = controller.GetInputMove() > 0 ? 0 : controller.GetInputMove() == 0 ? 1 : 2;
        actionSegment[1] = controller.GetInputRotation() > 0 ? 0 : controller.GetInputRotation() == 0 ? 1 : 2;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = trackSubGoals.GetNextSubGoal(controller.gameObject.transform).transform.position - controller.gameObject.transform.position;
        sensor.AddObservation(diff.normalized);

        sensor.AddObservation(controller.gameObject.transform.localRotation.normalized);

        sensor.AddObservation(Vector3.Dot(controller.gameObject.transform.forward.normalized, trackSubGoals.GetNextSubGoal(controller.gameObject.transform).transform.forward.normalized));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveInput = 0f;
        float turnInput = 0f;

        if (controller.moveSpeed < 5)
            AddReward(-1f);

        if (controller.turnInput == 0)
            AddReward(1f);

        switch (actions.DiscreteActions[0])
        {
            case 0:
                moveInput = 0.05f;
                break;
            case 1:
                moveInput = 0f;
                break;
            case 2:
                moveInput = -0.05f;
                break;
        }

        switch (actions.DiscreteActions[1])
        {
            case 0:
                turnInput = 1f;
                break;
            case 1:
                turnInput = 0f;
                break;
            case 2:
                turnInput = -1f;
                break;
        }

        controller.UpdateInputs(
           moveInput,
           turnInput);
    }
}
