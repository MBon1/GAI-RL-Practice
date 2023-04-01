using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class QLMazeAgent : Agent
{
    /*[SerializeField] private int gridSize;
    [SerializeField] private Vector2Int startingPosition;
    [SerializeField] private Vector2Int goalPosition;

    private Vector2Int currentPosition;
    private int currentDirection;


    public override void AgentReset()
    {
        // Reset the agent to starting position
        currentPosition = startingPosition;

        // Reset current direction
        currentDirection = 0;
    }

    public override void CollectObservations()
    {
        // Add current position and direction as observations
        AddVectorObs(currentPosition.x);
        AddVectorObs(currentPosition.y);
        AddVectorObs(currentDirection);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Get the move action
        int moveAction = Mathf.FloorToInt(vectorAction[0]);

        // Determine the move direction based on the move action
        Vector2Int moveDirection = Vector2Int.zero;
        switch (moveAction)
        {
            case 0:
                moveDirection = Vector2Int.up;
                break;
            case 1:
                moveDirection = Vector2Int.right;
                break;
            case 2:
                moveDirection = Vector2Int.down;
                break;
            case 3:
                moveDirection = Vector2Int.left;
                break;
        }

        // Calculate the new position based on the current position and move direction
        Vector2Int newPosition = currentPosition + moveDirection;

        // Move the agent to the new position if it is within the bounds of the tile grid
        if (newPosition.x >= 0 && newPosition.x < gridSize && newPosition.y >= 0 && newPosition.y < gridSize)
        {
            currentPosition = newPosition;
        }

        // Set the current direction to the move action
        currentDirection = moveAction;

        // Give a negative reward for each step taken
        AddReward(-1f);

        // Check if the agent has reached the goal position
        if (currentPosition == goalPosition)
        {
            // Give a positive reward for reaching the goal
            AddReward(1f);

            // End the episode
            Done();
        }
    }

    public override float[] Heuristic()
    {
        // Allow the agent to be controlled with arrow keys
        var action = new float[1];
        action[0] = 4;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            action[0] = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            action[0] = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            action[0] = 2;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            action[0] = 3;
        }
        return action;
    }*/
}
