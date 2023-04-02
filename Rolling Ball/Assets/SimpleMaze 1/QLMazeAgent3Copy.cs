using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class QLMazeAgent3Copy : Agent
{
    private Rigidbody rBody;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = this.transform.localPosition;
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = startPosition;
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
            SetReward(100.0f);
            EndEpisode();
        }

        //SetReward(-1.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.NameToLayer("Wall") == collision.gameObject.layer)
        {
            SetReward(-100.0f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (LayerMask.NameToLayer("Wall") == collision.gameObject.layer)
        {
            SetReward(-100.0f);
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}