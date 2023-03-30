using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLAgent : MonoBehaviour
{
    // // Stuff from the original code
    // public float speed = 15.0f;
    // public float rotationSpeed = 100.0f;

    // // Q-learning parameters
    // public float learningRate = 0.1f;
    // public float discountFactor = 0.99f;
    // public float explorationRate = 0.1f;

    // // State space
    // private Vector3 position;
    // private Vector3 velocity;
    // private float orientation;

    // // Action space
    // private float[] actions = {-1.0f, 0.0f, 1.0f};

    // // Q-values
    // private Dictionary<string, Dictionary<float, float>> qTable = new Dictionary<string, Dictionary<float, float>>();

    // // Raycasting parameters
    // public float raycastDistance = 1.0f;  /* ASSIGNMENT: Figure out what this is supposed to be */
    // public LayerMask obstacleLayer;

    // // Reward function
    // private float GetReward()
    // {
    //     /* ASSIGNMENT: Make this work properly with more than one straight ray */

    //     /* NOTE: It's doing this based on the ray, not the collision, which is weird, but it actually makes sense */
    //     /*       In fact, it could stand to reason that we should instead get the position of the hit and assign  */
    //     /*       rewards based on how close the hit is to the center of the gap. That might work even better.     */

    //     RaycastHit hit;

    //     bool targetHit = Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, targetLayer);
    //     bool wallHit = Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, wallLayer);
        
    //     if (targetHit)
    //     {
    //         if (hit.collider.CompareTag("target"))
    //         {
    //             // The ray hit a gap
    //             return 1.0f;
    //         }
    //     }
    //     else if (wallHit)
    //     {
    //         if (hit.collider.CompareTag("wall"))
    //         {
    //             // The ray hit an obstacle
    //             return -1.0f;
    //         }
    //     }

    //     // NOTE: if this has happened, something has probably gone wrong and it is likely looking over or under the wall

    //     // Nothing happened; default reward
    //     return 0.0f;
    // }

    // // Convert state to string for use as dictionary key
    // private string StateToString(Vector3 position, Vector3 velocity, float orientation)
    // {
    //     /* ASSIGNMENT: Truncate the float vals somehow because floating point equivalence is dog shit */
    //     /*             Maybe make it one decimal point or something to match approximate equivalence  */

    //     return position.ToString() + velocity.ToString() + orientation.ToString();
    // }

    // // Get highest Q-value for a state
    // private float MaxQ(string state)
    // {
    //     float maxQ = float.MinValue;

    //     if (qTable.ContainsKey(state))
    //     {
    //         foreach (float action in actions)
    //         {
    //             if (qTable[state][action] > maxQ)
    //             {
    //                 maxQ = qTable[state][action];
    //             }
    //         }
    //     }

    //     return maxQ;
    // }

    // // Get action with highest Q-value for a state
    // private float MaxQAction(string state)
    // {
    //     float maxQ = float.MinValue;
    //     float maxAction = 0.0f;

    //     if (qTable.ContainsKey(state))
    //     {
    //         foreach (float action in actions)
    //         {
    //             if (qTable[state][action] > maxQ)
    //             {
    //                 maxQ = qTable[state][action];
    //                 maxAction = action;
    //             }
    //         }
    //     }

    //     return maxAction;
    // }

    // // Get Q-value for a state-action pair
    // private float GetQValue(Vector3 position, Vector3 velocity, float orientation, float action)
    // {
    //     string state = StateToString(position, velocity, orientation);

    //     if (qTable.ContainsKey(state) && qTable[state].ContainsKey(action))
    //     {
    //         return qTable[state][action];
    //     }
        
    //     // The default Q-value
    //     return 0.0f;
    // }

    // // Set Q-value for a state-action pair
    // private void SetQValue(Vector3 position, Vector3 velocity, float orientation, float action, float value)
    // {
    //     string state = StateToString(position, velocity, orientation);

    //     if (!qTable.ContainsKey(state))
    //     {
    //         qTable[state] = new Dictionary<float, float>();
    //     }

    //     qTable[state][action] = value;
    // }

    // // Q-learning update
    // private void QLearningUpdate(float action, float reward, Vector3 newPosition, Vector3 newVelocity, float newOrientation)
    // {
    //     string state = StateToString(newPosition, newVelocity, newOrientation);

    //     float maxQ = MaxQ(state);
    //     float oldQ = GetQValue(position, velocity, orientation, action);
    //     float newQ = (1.0f - learningRate) * oldQ + learningRate * (reward + discountFactor * maxQ);

    //     SetQValue(position, velocity, orientation, action, newQ);
    // }

    // // Choose an action based on the current state
    // private float ChooseAction()
    // {
    //     if (Random.value < explorationRate)
    //     {
    //         return actions[Random.Range(0, actions.Length)];
    //     }

    //     return MaxQAction(StateToString(position, velocity, orientation));
    // }

    // // Update is called once per frame
    // void FixedUpdate()
    // {
    //     // Save current state
    //     Vector3 oldPosition = position;
    //     Vector3 oldVelocity = velocity;
    //     float oldOrientation = orientation;

    //     // Choose action
    //     float action = ChooseAction();

    //     // Update position and orientation based on action
    //     transform.Rotate(-action * Vector3.right * rotationSpeed * Time.deltaTime);
    //     transform.Translate(Vector3.forward * speed * Time.deltaTime);

    //     // Get new state
    //     position = transform.position;
    //     velocity = (position - oldPosition) / Time.deltaTime;
    //     orientation = transform.eulerAngles.x;

    //     // Get reward
    //     float reward = GetReward();

    //     // Update Q-values
    //     QLearningUpdate(action, reward, position, velocity, orientation);

    //     // Check for collision
    //     if (reward == -1.0f) {
    //         transform.position = oldPosition;
    //         position = oldPosition;
    //         velocity = oldVelocity;
    //         orientation = oldOrientation;
    //     }
    // }
}
