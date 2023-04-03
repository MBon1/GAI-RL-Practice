# GAI-RL-Practice
================================================================

+----------------------+
| RL PRACTICE HOMEWORK |
+----------------------+

================================================================

GROUP MEMBERS

- Matthew Bonnecaze  (bonnem3)
- Justin Hung        (hungj2)
- Phillip Stapleton  (staplp2)

================================================================

GITHUB REPOSITORY

- https://github.com/MBon1/GAI-RL-Practice

================================================================

Q-LEARNING TRAINING AND RESULTS

  For Q-learning scenes, parameters for training can be changed by modifying the Max_steps, Max_iterations, and Report_on_Iteration variables.

  Max_steps is the number of steps the agent is allowed to make in one iteration before a restart is forced. The scene will end if the max number of steps is used or if the agent reaches the goal.

  Max_iterations is the number of iterations (episodes) that the agent will carry out before training is complete. For all of our scenes, the default value is around 2000 iterations; though some scenes (like the 10x10 maze) may converge much sooner than that.

  Report_on_Iteration is the period over which reports will be recorded into the results CSV file (i.e. every 5 iterations). The values recorded between the first and last iteration in this period will be averaged. For example, if Report_on_Iteration is 5, an entry stating the average score over every period of five iterations will be reported.

  The results for Q-learning runs are saved as CSV files and can be found in Assets/Results. These files, which store the iteration number in the A column and the cumulative reward value in the B column, can be opened in an application like Microsoft Excel, Apple Numbers, or Google Sheets, where you will be able to convert the data into a graph.


================================================================

+------------+
| MAZE SCENE |
+------------+

+ OVERVIEW AND OBJECTIVE

  In our maze scene, the agent begins in the bottom-left corner (0,0) of the maze. The goal of the agent is to reach the goal of the maze located in the top corner (nx - 1, nz - 1), where nx and nz are the number of rows and columns in the maze, respectively. Our sample scene is a 10x10 maze with five obstacles.

  The agent is represented as a yellow cube. The goal is represented by a tan cube in the top right corner of the maze. Obstacles are represented by black wireframe cubes.


+ SCORING DETAILS

  To disincentivize the agent from simply wandering around, we punish the agent with -1 points for moving to another square. 

  Because of the way that our Q-learning and mlagents (PPO) algorithms work, there are slight differences in punishment for hitting an obstacle. Hitting an obstacle creates a -100 reward for Q-learning implementation Whereas a -1 reward point is given for the mlagents (PPO) implementation.

  Reaching the goal is the only way for the agent to actually gain a positive reward. Reaching the goal gives the agent a reward of 100, which is large enough to incentivise the agent to seek out the end of the maze rather than waiting out the clock and using all its given moves.


+ RUNNING THE SCENE 

Please note that SimpleMazeMLA.unity is an unused scene.

Q Learning:

  In Assets/SimpleMaze/SimpleMaze.unity (NOT SimpleMazeMLA.unity or SimpleMazeMLA2.unity), you can modify the Alpha and Gamma values for the Q-Learning algorithm as well has how many iterations to run the training for in the QL Maze component on the Player game object. Press the play button and run the scene to begin training. When the scene is run, a CSV is created to report the training results and will be updated with the reported reward values at the specified rate. The CSV will be located in Assets/Results/QLMaze named "QLMaze_" followed by the year, month, day, hour, minute, and second the file was created. 

mlagents (PPO):

  In a Python/Anaconda terminal, navigate to the MLAgents folder in the GitHub repository. Run the mlagents-learn command using the trainer_config.yaml. The training environment is located in the scene Assets/Results/SimpleMazeMLA2.unity (NOT SimpleMaze.unity or SimpleMazeMLA.unity). Make sure that the Behavior Name in the Behavior Parameter Component on all Players matches the one you want to use in the trainer_config.yaml file. There are three provided behaviors: "QLMaze" (the one used to produce the data seen in Graph 1 of the ML-Agents results subsection of the Findings section of Maze Scene), "QLMaze_1" (the behavior used to produce the data seen in Graph 2, the Alpha test, of the ML-Agents results subsection of the Findings section of Maze Scene), and "QLMaze_2 (the behavior used to produce the data seen in Graph 3, the Gamma test, of the ML-Agents results subsection of the Findings section of Maze Scene). Press the play button and run the scene to begin training. 


================================================================

+-------------------+
| ROLLER BALL SCENE |
+-------------------+

+ OVERVIEW AND OBJECTIVE

  The Rollerball environment is based off of the Rollerball environment from the tutorial “Making a New Learning Environment” in the ml-agents GitHub documentation (https://github.com/Unity-Technologies/ml-agents/blob/release_1/docs/Learning-Environment-Create-New.md). In the Rollerball environment, there is an agent (a red ball) and a target (a green cube) on a floor. The agent is trying to get to the target without falling off of the floor. While in the original Rollerball the target’s position is randomly determined at the start of each iteration, in our episode, the agent’s position is randomly determined at the start of each episode.


+ RUNNING THE SCENE

Q-Learning:

  In Assets/Rollerball/RollerballQL.unity, you can modify the Alpha and Gamma values for the Q-Learning algorithm as well has how many iterations to run the training for in the QL Roller Agent component on the RollerAgent game object in the training environment. Press the play button and run the scene to begin training. When the scene is run, a CSV is created to report the training results and will be updated with the reported reward values at the specified rate. The CSV will be located in Assets/Results/QLRollerball named "QLRollerball_" followed by the year, month, day, hour, minute, and second the file was created.

mlagents (PPO):

  In a Python/Anaconda terminal, navigate to the MLAgents folder in the GitHub repository. Run the mlagents-learn command using the trainer_config.yaml. The training environment is located in the scene Assets/Rollerball/RollerballMLA.unity. Make sure that the Behavior Name in the Behavior Parameter Component on all RollerAgents matches the one you want to use in the trainer_config.yaml file. There are three provided behaviors: "RollerBallMLA" (the one used to produce the data seen in Graph 1 of the ML-Agents results subsection of the Findings section of Roller Ball Scene), "RollerBallMLA_1" (the behavior used to produce the data seen in Graph 2, the Alpha test, of the ML-Agents results subsection of the Findings section of Roller Ball Scene), and "RollerBallMLA_2 (the behavior used to produce the data seen in Graph 3, the Gamma test, of the ML-Agents results subsection of the Findings section of Roller Ball Scene). Press the play button and run the scene to begin training. 


================================================================

IMPORTANT SCRIPTS

- QLMaze.cs		  (Q-Learning script in SimpleMaze.unity)
- QLMazeAgent3.cs	  (ML-Agent script in SimpleMazeMLA2.unity)
- QLRollerAgent.cs  (Q-Learning script in RollerballQL.unity)
- RollerAgent.cs	  (ML-Agent script in RollerballMLA.unity)
- QLHallway.cs	  (Q-Learning script in SimpleHallway.unity)

================================================================


For the full Training Results and Analysis, please see the "README and Analysis" PDF.
