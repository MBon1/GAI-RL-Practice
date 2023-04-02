using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;


public class QLMazeAgent4 : Agent
{
    public Transform Target;
    public Transform[] Obstacles;
    private Rigidbody rBody;
    private Vector3 startPosition;
    void Start()
    {
        startPosition = this.transform.localPosition;
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = startPosition;
        rBody.velocity = Vector3.zero;
        rBody.angularVelocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        foreach (Transform obstacle in Obstacles)
        {
            sensor.AddObservation(obstacle.localPosition);
        }

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
        /*float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);*/

        // Reached target
        /*if (distanceToTarget < 1.42f)
        {
            AddReward(100.0f);
            EndEpisode();
        }*/

        AddReward(-1 / MaxStep);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("wall"))
        {
            AddReward(-1f);
        }

        if (collision.collider.CompareTag("target"))
        {
            AddReward(10.0f);
            EndEpisode();
        }

        /*if (LayerMask.NameToLayer("Wall") == collision.gameObject.layer)
        {
            AddReward(-0.5f);
            //EndEpisode();
        }*/
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("wall"))
        {
            AddReward(-1f);
        }
    }

    /*private void OnCollisionStay(Collision collision)
    {
        if (LayerMask.NameToLayer("Wall") == collision.gameObject.layer)
        {
            SetReward(-100.0f);
        }
    }*/

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}