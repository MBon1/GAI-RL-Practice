﻿using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class QLMazeAgent2 : Agent
{
    private Rigidbody rBody;
    private Vector3 originPosition;

    void Start()
    {
        originPosition = this.transform.localPosition;
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = originPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }

    public float speed = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(100f);
            EndEpisode();
            return;
        }

        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }

        SetReward(-1);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.NameToLayer("Wall") == collision.gameObject.layer)
        {
            SetReward(-100f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (LayerMask.NameToLayer("Wall") == collision.gameObject.layer)
        {
            SetReward(-100f);
        }
    }
}