using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QLRollerAgent : MonoBehaviour
{
    const string PANIC = "panic";

    // Q-learning variables
    // private Dictionary<Tuple<int, int>, Dictionary<string, float>> q_table;  // lmao this did not work
    private Dictionary<string, Dictionary<string, float>> q_table;
    private float learning_rate = 0.8f;
    private float discount_factor = 0.95f;
    private float exploration_rate = 0.2f;

    // Maze variables
    private int nx;
    private int ny;
    private Tuple<int, int> start_pos;
    private Tuple<int, int> goal_pos;

    // Agent variables
    private Tuple<int, int> current_pos;
    private string[] actions = { "up", "down", "left", "right" };

    // Unity variables
    public GameObject agent;
    public GameObject goal;

    // Timestep regulation variables
    /* ASSIGNMENT: only call Update() x times per second */

    // Episode regulation variables
    public int max_steps = 500;
    int curr_step = 0;

    // Iterations
    public int max_iterations = 20;
    int curr_iterations = 0;

    // Data Reporting
    public int report_on_iteration = 5;
    string file_path = "";
    float iteration_reward = 0;
    float[] iteration_rewards;

    void Start()
    {
        // Initialize maze variables
        nx = 10; // set to desired size
        ny = 10; // set to desired size
        start_pos = new Tuple<int, int>(UnityEngine.Random.Range(0, nx), UnityEngine.Random.Range(0, ny)); // set to desired start position
        goal_pos = new Tuple<int, int>(Mathf.FloorToInt(goal.transform.position.x), Mathf.FloorToInt(goal.transform.position.z)); // set to desired goal position

        // Initialize Q-learning variables
        // q_table = new Dictionary<Tuple<int, int>, Dictionary<string, float>>();  // lmao nope
        q_table = new Dictionary<string, Dictionary<string, float>>();
        for (int i = -1; i <= nx; i++)
        {
            for (int j = -1; j <= ny; j++)
            {
                // Tuple<int, int> state = new Tuple<int, int>(i, j);  // this didn't work
                string state = ConvertTupleToString(new Tuple<int, int>(i, j));
                Dictionary<string, float> state_actions = new Dictionary<string, float>();
                foreach (string action in actions)
                {
                    state_actions.Add(action, 0.0f);
                }
                q_table.Add(state, state_actions);
            }
        }

        // Initialize agent variables
        current_pos = start_pos;

        // Set Up File I/O for data reporting
        SetUpIO();
        iteration_rewards = new float[report_on_iteration];
    }

    void Update()
    {
        if (max_iterations < curr_iterations)
        {
            return;
        }
        // Update agent position and Q-learning table
        Tuple<int, int> next_pos = ChooseAction();
        float reward = GetReward(next_pos);
        iteration_reward += reward;
        UpdateQTable(current_pos, next_pos, reward);
        current_pos = next_pos;
        agent.transform.position = new Vector3(current_pos.Item1 + 0.5f, 0.5f, current_pos.Item2 + 0.5f);

        // Keep track of the current step
        bool restart_needed = false;
        curr_step++;

        // Check if agent reached the goal
        if (current_pos.Equals(goal_pos))
        {
            Debug.Log("Agent reached the goal!");
            restart_needed = true;
        }
        else if (curr_step >= max_steps)
        {
            Debug.Log("Max steps exceeded!");
            restart_needed = true;
        }
        // Check if agent is "out of bounds"
        else if (!IsValidPosition(next_pos))
        {
            Debug.Log("Out of bounds!");
            restart_needed = true;
        }

        // Restart the episode
        if (restart_needed)
        {
            EndEpisode();
        }
    }

    private void EndEpisode()
    {
        iteration_rewards[curr_iterations % report_on_iteration] = iteration_reward;
        UpdateIterationResults();
        curr_step = 0;
        current_pos = new Tuple<int, int>(UnityEngine.Random.Range(0, nx), UnityEngine.Random.Range(0, ny));
        iteration_reward = 0;
        curr_iterations++;
    }

    Tuple<int, int> ChooseAction()
    {
        // Choose action with highest Q-value or explore randomly
        if (UnityEngine.Random.value < exploration_rate)
        {
            return GetRandomAction();
        }
        else
        {
            return GetBestAction(current_pos);
        }
    }

    Tuple<int, int> GetRandomAction()
    {
        // Choose random action among available actions
        List<string> available_actions = GetAvailableActions(current_pos);
        int random_index = UnityEngine.Random.Range(0, available_actions.Count);
        return GetNextPosition(current_pos, available_actions[random_index]);
    }

    Tuple<int, int> GetBestAction(Tuple<int, int> curr_pos)
    {
        // Choose action with highest Q-value among available actions
        List<string> available_actions =
        GetAvailableActions(curr_pos);
        string best_action = "";
        float best_q_value = float.NegativeInfinity;
        foreach (string action in available_actions)
        {
            Tuple<int, int> next_pos = GetNextPosition(curr_pos, action);
            float q_value = GetQValue(ConvertTupleToString(curr_pos), action);
            if (q_value > best_q_value)
            {
                best_action = action;
                best_q_value = q_value;
            }
        }
        return GetNextPosition(curr_pos, best_action);
    }

    List<string> GetAvailableActions(Tuple<int, int> pos)
    {
        // Get list of available actions at given position
        List<string> available_actions = new List<string>();
        foreach (string action in actions)
        {
            available_actions.Add(action);
        }
        return available_actions;
    }

    bool IsValidPosition(Tuple<int, int> pos)
    {
        // Check if position is within bounds and not an obstacle
        if (pos.Item1 < 0 || pos.Item1 >= nx || pos.Item2 < 0 || pos.Item2 >= ny)
        {
            return false;
        }
        return true;
    }

    Tuple<int, int> GetNextPosition(Tuple<int, int> pos, string action)
    {
        // Get next position given current position and action
        int x = pos.Item1;
        int y = pos.Item2;
        if (action == "up")
        {
            y += 1;
        }
        else if (action == "down")
        {
            y -= 1;
        }
        else if (action == "left")
        {
            x -= 1;
        }
        else if (action == "right")
        {
            x += 1;
        }
        return new Tuple<int, int>(x, y);
    }

    // float GetQValue(Tuple<int, int> state, string action)
    float GetQValue(string state, string action)
    {
        // Get Q-value for given state-action pair
        Dictionary<string, float> state_actions = q_table[state];
        return state_actions[action];
    }

    float GetReward(Tuple<int, int> next_pos)
    {
        // Get reward for transitioning from current position to next position
        if (next_pos.Equals(goal_pos))
        {
            return 100.0f;
        }
        else if (!IsValidPosition(next_pos))
        {
            return -100.0f;
        }
        else
        {
            return -1.0f;
        }
    }

    void UpdateQTable(Tuple<int, int> state, Tuple<int, int> next_state, float reward)
    {
        // Update Q-table using Q-learning algorithm
        string qKey = GetProjectedDirection(state, GetBestAction(current_pos));
        if (qKey == PANIC)
            return;
        float q_value = GetQValue(ConvertTupleToString(state), qKey);
        qKey = GetProjectedDirection(next_state, GetBestAction(next_state));
        if (qKey == PANIC)
            return;
        float max_q_value = GetQValue(ConvertTupleToString(next_state), qKey);
        float updated_q_value = q_value + learning_rate * (reward + discount_factor * max_q_value - q_value);
        q_table[ConvertTupleToString(state)][GetProjectedDirection(state, GetBestAction(current_pos))] = updated_q_value;
    }

    // Parse the direction based on the next move
    string GetProjectedDirection(Tuple<int, int> curr, Tuple<int, int> best_action)
    {
        //Debug.Log($"{ConvertTupleToString(curr)}-{ConvertTupleToString(best_action)}");

        int sx = curr.Item1;
        int sz = curr.Item2;

        int tx = best_action.Item1;
        int tz = best_action.Item2;

        if (tx < sx)
        {
            return "left";
        }
        else if (tx > sx)
        {
            return "right";
        }
        else if (tz < sz)
        {
            return "down";
        }
        else if (tz > sz)
        {
            return "up";
        }

        // Something has gone wrong
        //Debug.Log(PANIC);
        return PANIC;
    }

    // Turns out that you can't use tuples as hash values / keys or things break
    string ConvertTupleToString(Tuple<int, int> tup)
    {
        return $"{tup.Item1},{tup.Item2}";
    }

    void SetUpIO()
    {
        string directory = Application.dataPath + "/Results/QLRollerball";
        System.IO.Directory.CreateDirectory(directory);
        System.DateTime dt = System.DateTime.Now;
        file_path = directory + "/QLRollerball_" + dt.Year + " -" + dt.Month + "-" + dt.Day + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
        Debug.Log(file_path);

        string header = "Steps,Environment/Cumulative Reward";
        AppendDataToFile(header);
    }

    void AppendDataToFile(float reward)
    {
        // Steps
        // Environment/Cumulative Reward
        string contents = $"{curr_iterations},{reward}";

        AppendDataToFile(contents);
    }

    void AppendDataToFile(string contents)
    {
        System.IO.File.AppendAllText(file_path, contents + "\n");
    }

    void UpdateIterationResults()
    {
        // Check if results should be reported
        if ((curr_iterations > 0 && curr_iterations % report_on_iteration == 0) || curr_iterations == max_iterations)
        {
            float sum = 0;

            for (int i = 0; i < report_on_iteration; i++)
            {
                sum += iteration_rewards[i];
            }
            AppendDataToFile(sum / report_on_iteration);
            iteration_rewards = new float[report_on_iteration];
        }
    }
}