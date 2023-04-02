using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class QLMaze_MLA_AgentCopy : Agent
{
    /*private int nx = 10;
    private int ny = 10;

    // Unity variables
    public GameObject agent;
    public GameObject goal;
    public GameObject[] obstacles_go;
    internal Vector3 initPosition;

    private void Start()
    {
        initPosition = this.transform.position;
    }

    public override void OnEpisodeBegin()
    {
        this.transform.position = initPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent position
        sensor.AddObservation(this.transform.position);
        
        // Possible new positions for Agent 
        sensor.AddObservation(this.transform.position + new Vector3(1, 0, 0));
        sensor.AddObservation(this.transform.position - new Vector3(1, 0, 0));
        sensor.AddObservation(this.transform.position + new Vector3(0, 0, 1));
        sensor.AddObservation(this.transform.position - new Vector3(0, 0, 1));
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Get Agent Position
        Vector3Int playerPosition = new Vector3Int(Mathf.FloorToInt(vectorAction[0]), Mathf.FloorToInt(vectorAction[1]), Mathf.FloorToInt(vectorAction[2]));

        Vector3 goalPosition = goal.transform.position;
        Vector3Int goalPositionFloored = new Vector3Int(Mathf.FloorToInt(goalPosition.x), Mathf.FloorToInt(goalPosition.y), Mathf.FloorToInt(goalPosition.z));

        // Reached Target; reward by +100
        if (playerPosition == goalPositionFloored)
        {
            AddReward(100);
            EndEpisode();
            return;
        }

        // Determine if an obstacle was hit
        bool hitObstacle = false;
        foreach (GameObject obstacle in obstacles_go)
        {
            Vector3 obstaclePosition = obstacle.transform.position;
            Vector3Int obstaclePositionFloored = new Vector3Int(Mathf.FloorToInt(obstaclePosition.x), Mathf.FloorToInt(obstaclePosition.y), Mathf.FloorToInt(obstaclePosition.z));
            if (obstaclePositionFloored == playerPosition)
            {
                hitObstacle = false;
            }
        }
        
        if (hitObstacle)
        {
            // Reached an obstacle; punish by -100
            AddReward(-100);
            EndEpisode();
            return;
        }


        // Get Agent Right Position
        Vector3Int rightPosition = new Vector3Int(Mathf.FloorToInt(vectorAction[3]), Mathf.FloorToInt(vectorAction[4]), Mathf.FloorToInt(vectorAction[5]));
        // Get Agent Left Position
        Vector3Int leftPosition = new Vector3Int(Mathf.FloorToInt(vectorAction[6]), Mathf.FloorToInt(vectorAction[7]), Mathf.FloorToInt(vectorAction[8]));
        // Get Agent Forward Position
        Vector3Int forwardPosition = new Vector3Int(Mathf.FloorToInt(vectorAction[9]), Mathf.FloorToInt(vectorAction[10]), Mathf.FloorToInt(vectorAction[11]));
        // Get Agent Backward Position
        Vector3Int backwardPosition = new Vector3Int(Mathf.FloorToInt(vectorAction[12]), Mathf.FloorToInt(vectorAction[13]), Mathf.FloorToInt(vectorAction[14]));

        Vector3Int[] positions = new Vector3Int[]
        {
            forwardPosition, rightPosition, backwardPosition, leftPosition
        };


        // Move
        List<Vector3Int> validPositions = new List<Vector3Int>();
        foreach (Vector3Int position in positions) {
            // Check if position is at goal
            if (position == goalPositionFloored)
            {
                // Set Player Position to be position
                // Return
                this.transform.position = position + new Vector3(0.5f, 0.5f, 0.5f);
                return;
            }
            
            bool valid = true;
            foreach (GameObject obstacle in obstacles_go)
            {
                Vector3 obstaclePosition = obstacle.transform.position;
                Vector3Int obstaclePositionFloored = new Vector3Int(Mathf.FloorToInt(obstaclePosition.x), Mathf.FloorToInt(obstaclePosition.y), Mathf.FloorToInt(obstaclePosition.z));
                if (obstaclePositionFloored == position)
                {
                    valid = false;
                }
            }
            if (valid && position.x >= 0 && position.x < nx && position.z >= 0 && position.z < ny)
            {
                validPositions.Add(position);
            }
        }

        this.transform.position = validPositions[Random.Range(0, validPositions.Count)] + new Vector3(0.5f, 0.5f, 0.5f);

        // Take step; punish by -1
        AddReward(-1);
    }*/
}
