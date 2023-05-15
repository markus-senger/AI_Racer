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
    [SerializeField] private SphereCollider motorSphere;

    private Vector3 startPosSphere;

    private void Awake()
    {
        startPosSphere = motorSphere.gameObject.transform.position;
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
        AddReward(-1f);
    }

    private void OnCarColliderStay(object sender, EventArgs e)
    {
        AddReward(-0.1f);
    }

    private void OnCarCorrectSubGoal(object sender, EventArgs e)
    {
        AddReward(0.5f);
    }

    private void OnCarWrongSubGoal(object sender, EventArgs e)
    {
        AddReward(-1f);
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Start Ep: " + startPosSphere);

        colliderCheck.GetComponent<BoxCollider>().enabled = false;
        motorSphere.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        DecisionRequester decisionRequester = GetComponent<DecisionRequester>();
        if (decisionRequester != null)
        {
            Destroy(decisionRequester);
        }

        controller.ResetValues();
        trackSubGoals.ResetSubGoals();
        motorSphere.gameObject.transform.position = startPosSphere; // + new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, UnityEngine.Random.Range(-5f, 5f));
        transform.rotation = Quaternion.Euler(0,0,0);
        StartCoroutine(RespawnDecisionRequester(2f));
    }

    private IEnumerator RespawnDecisionRequester(float delay)
    {
        yield return new WaitForSeconds(delay);

        motorSphere.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        colliderCheck.GetComponent<BoxCollider>().enabled = true;
        var decisionRequester = gameObject.AddComponent<DecisionRequester>();
        decisionRequester.DecisionPeriod = 10;
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
