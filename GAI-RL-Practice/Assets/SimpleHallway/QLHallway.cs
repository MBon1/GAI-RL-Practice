using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLHallway : MonoBehaviour
{
    const string PANIC = "panic";

    // Q-learning variables
    private Dictionary<string, Dictionary<string, float>> q_table;
    private float learning_rate = 0.8f;
    private float discount_factor = 0.95f;
    private float exploration_rate = 0.2f;

    // Maze variables
    private int nx;
    private int nz;
    private Tuple<int, int> start_pos;
    private Tuple<int, int> sign_pos;
    private Tuple<int, int> goal_a_pos;
    private Tuple<int, int> goal_b_pos;
    private Tuple<int, int> no_mans_land_pos;

    // Agent variables
    private Tuple<int, int> current_pos;
    private string[] actions = { "up", "down", "left", "right" };

    // Unity variables
    public GameObject agent;
    public GameObject blue_sign;
    public GameObject red_sign;
    public GameObject blue_goal;
    public GameObject red_goal;

    // I am a dumbass
    bool blue_is_a;    // the a goal is blue -- I know this naming convention is bad :(
    int blue_is_goal;  // 0 == no; 1 == yes; 2 == don't know yet

    // Episode regulation variables
    public int max_steps = 50;
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
        // Initialize no man's land (where the unused sign goes)
        no_mans_land_pos = new Tuple<int, int>(-3, 3);

        // Initialize maze variables
        nx = 3; // set to desired size
        nz = 10; // set to desired size
        start_pos = new Tuple<int, int>(0, 0); // set to desired start position
        goal_a_pos = new Tuple<int, int>(0, (nz - 1));
        goal_b_pos = new Tuple<int, int>((nx - 1), (nz - 1));
        
        // Initialize Q-learning variables
        q_table = new Dictionary<string, Dictionary<string, float>>();
        for (int i = 0; i < nx; i++)
        {
            for (int j = 0; j < nz; j++)
            {
                Tuple<int, int> pos_ij = new Tuple<int, int>(i, j);

                // state is current position + is the left goal blue + is the target goal blue
                string state_1 = GenerateState(pos_ij, false, 0);  // left is red; red goal
                string state_2 = GenerateState(pos_ij, false, 1);  // left is red; blue goal
                string state_3 = GenerateState(pos_ij, false, 2);  // left is red; unknown goal
                string state_4 = GenerateState(pos_ij, true, 0);   // left is blue; red goal
                string state_5 = GenerateState(pos_ij, true, 1);   // left is blue; blue goal
                string state_6 = GenerateState(pos_ij, true, 2);   // left is blue; unknown goal

                Dictionary<string, float> state_actions = new Dictionary<string, float>();

                foreach (string action in actions)
                {
                    state_actions.Add(action, 0.0f);  // one for each direction
                }

                q_table.Add(state_1, state_actions);
                q_table.Add(state_2, state_actions);
                q_table.Add(state_3, state_actions);
                q_table.Add(state_4, state_actions);
                q_table.Add(state_5, state_actions);
                q_table.Add(state_6, state_actions);
            }
        }

        // Randomize the board and reset position;
        ItsRewindTime();

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

        // Look for the sign
        if (blue_is_goal == 2)  // if the goal color is unknown
        {
            LookForSign(current_pos);  // see if the sign is next to the current and update if so
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
        if (current_pos.Equals(goal_a_pos) || (current_pos.Equals(goal_b_pos)))
        {
            Debug.Log("Reached a goal! Restarting Episode!");
            restart_needed = true;
        }
        else if (curr_step >= max_steps)
        {
            Debug.Log("Max steps exceeded! Restarting Episode!");
            restart_needed = true;
        }

        // Restart the episode
        if (restart_needed)
        {
            iteration_rewards[curr_iterations % report_on_iteration] = iteration_reward;
            UpdateIterationResults();
            iteration_reward = 0;
            curr_iterations++;

            // Actually restart everything
            ItsRewindTime();
        }
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
            float q_value = GetQValue(GenerateState(curr_pos, blue_is_a, blue_is_goal), action);
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
            Tuple<int, int> next_pos = GetNextPosition(pos, action);
            if (IsValidPosition(next_pos))
            {
                available_actions.Add(action);
            }
        }
        return available_actions;
    }

    bool IsValidPosition(Tuple<int, int> pos)
    {
        // Check if position is within bounds and not an obstacle
        if (pos.Item1 < 0 || pos.Item1 >= nx || pos.Item2 < 0 || pos.Item2 >= nz)
        {
            return false;
        }

        // if (pos.Equals(sign_pos))
        // {
        //     return false;  // Thanks for the idea, ChatGPT!
        // }

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
        if (next_pos.Equals(goal_a_pos))
        {
            if (blue_is_a && (blue_is_goal == 1))
            {
                // curr goal is A goal, A goal is blue, target is blue goal
                Debug.Log("CORRECT");
                return 100.0f;
            }
            else if(!blue_is_a && (blue_is_goal == 0))
            {
                Debug.Log("CORRECT");
                return 100.0f;
            }

            return -100.0f;
        }
        else if (next_pos.Equals(goal_b_pos))
        {
            if (!blue_is_a && (blue_is_goal == 1))
            {
                // curr goal is B goal, B goal is blue, target is blue goal
                Debug.Log("CORRECT");
                return 100.0f;
            }
            else if (blue_is_a && (blue_is_goal == 0))
            {
                Debug.Log("CORRECT");
                return 100.0f;
            }

            return -100.0f;
        }        
        else
        {
            return -1.0f;
        }

        // NOTE THAT UNSURE GUESSES WHERE THE SIGN HAS NOT BEEN FOUND COUNT AS INCORRECT
    }

    void UpdateQTable(Tuple<int, int> curr_pos, Tuple<int, int> next_pos, float reward)
    {
        // // Update Q-table using Q-learning algorithm
        // string qKey = GetProjectedDirection(state, GetBestAction(current_pos));
        // if (qKey == PANIC)
        //     return;
        // float q_value = GetQValue(ConvertTupleToString(state), qKey);
        // qKey = GetProjectedDirection(next_state, GetBestAction(next_state));
        // if (qKey == PANIC)
        //     return;
        // float max_q_value = GetQValue(ConvertTupleToString(next_state), qKey);
        // float updated_q_value = q_value + learning_rate * (reward + discount_factor * max_q_value - q_value);
        // q_table[ConvertTupleToString(state)][GetProjectedDirection(state, GetBestAction(current_pos))] = updated_q_value;

        string curr_state = GenerateState(curr_pos, blue_is_a, blue_is_goal);
        string next_state = GenerateState(next_pos, blue_is_a, blue_is_goal);

        // Debug.Log("Ahoy, matey!");

        string qKey = GetProjectedDirection(curr_pos, GetBestAction(curr_pos));
        float q_value = GetQValue(curr_state, qKey);
        qKey = GetProjectedDirection(next_pos, GetBestAction(next_pos));
        float max_q_value = GetQValue(next_state, qKey);
        float updated_q_value = q_value + learning_rate * (reward + discount_factor * max_q_value - q_value);

        q_table[curr_state][GetProjectedDirection(curr_pos, GetBestAction(curr_pos))] = updated_q_value;
    }

    // Parse the direction based on the next move
    string GetProjectedDirection(Tuple<int, int> curr, Tuple<int, int> best_action) {
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
        string directory = Application.dataPath + "/Results/QLHallway";
        System.IO.Directory.CreateDirectory(directory);
        System.DateTime dt = System.DateTime.Now;
        file_path = directory + "/q-l-maze-cumulative-reward" + dt.Year + " -" + dt.Month + "-" + dt.Day + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
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

    bool CoinFlipIsHeads()
    {
        return UnityEngine.Random.value > 0.5f;
    }

    void RandomizeGoalPositions()
    {
        bool blue_goal_is_goal_a = CoinFlipIsHeads();

        if (CoinFlipIsHeads())
        {
            blue_goal.transform.position = new Vector3(goal_a_pos.Item1 + 0.5f, 0.5f, goal_a_pos.Item2 + 0.5f);
            red_goal.transform.position = new Vector3(goal_b_pos.Item1 + 0.5f, 0.5f, goal_b_pos.Item2 + 0.5f);
            blue_is_a = true;
        }
        else
        {
            blue_goal.transform.position = new Vector3(goal_b_pos.Item1 + 0.5f, 0.5f, goal_b_pos.Item2 + 0.5f);
            red_goal.transform.position = new Vector3(goal_a_pos.Item1 + 0.5f, 0.5f, goal_a_pos.Item2 + 0.5f);
            blue_is_a = false;
        }
    }

    void RandomizeSignPosition()
    {
        int x = (int) UnityEngine.Random.Range(0.0f, nx - 0.00001f);
        int z = (int) UnityEngine.Random.Range(3.0f, 6.99999f);

        sign_pos = new Tuple<int, int>(x, z);

        bool sign_is_blue = CoinFlipIsHeads();

        if (sign_is_blue)
        {
            blue_sign.transform.position = new Vector3(sign_pos.Item1 + 0.5f, 0.5f, sign_pos.Item2 + 0.5f);
            red_sign.transform.position = new Vector3(no_mans_land_pos.Item1 + 0.5f, 0.5f, no_mans_land_pos.Item2 + 0.5f);
        }
        else
        {
            blue_sign.transform.position = new Vector3(no_mans_land_pos.Item1 + 0.5f, 0.5f, no_mans_land_pos.Item2 + 0.5f);
            red_sign.transform.position = new Vector3(sign_pos.Item1 + 0.5f, 0.5f, sign_pos.Item2 + 0.5f);
        }
    }

    void ItsRewindTime()
    {
        // (Re)initialize agent variables
        current_pos = start_pos;
        RandomizeGoalPositions();
        RandomizeSignPosition();
        
        // Dammit
        blue_is_goal = 2;
        curr_step = 0;
    }

    string GenerateState(Tuple<int, int> pos_ij, bool a_blue, int goal_blue)
    {
        string part_a = ConvertTupleToString(pos_ij);
        string part_b = a_blue ? "1" : "0";
        int part_c = goal_blue;

        return $"{part_a}-{part_b}-{part_c}";
    }

    void LookForSign(Tuple<int, int> curr_pos)
    {
        // only look for a sign if we don't know color goal is
        if (blue_is_goal == 2)
        {
            if (curr_pos.Item1 > 0)
            {
                // look left
                blue_is_goal = SignReport(new Tuple<int, int>(curr_pos.Item1 - 1, curr_pos.Item2));
            }

            if (curr_pos.Item1 < (nx - 1))
            {
                // look right
                blue_is_goal = SignReport(new Tuple<int, int>(curr_pos.Item1 + 1, curr_pos.Item2));
            }

            if (curr_pos.Item2 > 0)
            {
                // look down
                blue_is_goal = SignReport(new Tuple<int, int>(curr_pos.Item1, curr_pos.Item2 - 1));
            }

            if (curr_pos.Item2 < (nz - 1))
            {
                // look up
                blue_is_goal = SignReport(new Tuple<int, int>(curr_pos.Item1, curr_pos.Item2 + 1));
            }
        }

    }

    int SignReport(Tuple<int, int> subject_space)  // subject_space is the space being looked at
    {
        // Report 0 (sign is red), 1 (sign is blue), or 2 (no sign is visible)

        if (subject_space.Equals(sign_pos))
        {
            int ssx = subject_space.Item1;
            int ssz = subject_space.Item2;

            int bsx = (int) blue_sign.transform.position.x;
            int bsz = (int) blue_sign.transform.position.z;

            int rsx = (int) red_sign.transform.position.x;
            int rsz = (int) red_sign.transform.position.z;

            if (ssx == bsx && ssz == bsz)
            {
                return 1;
            }
            else if (ssx == rsx && ssz == rsz)
            {
                return 0;
            }   
        }

        return 2;
    }
}