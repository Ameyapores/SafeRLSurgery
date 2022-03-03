# Safe RL for surgery

<figure>
 <img src="images/Picture3.png" width="600"> 
<figure>
 
## Folders:
- Unity: contains the Unity project, which has been modified to slightly adjust the observation space, the reward function and other minor details (e.g., the delta movement)
- Testing: contains the files to test the trained model in a built environment (env_fullbody) or in the editor. To test the model in the builded env just run "main.py", for the editor option, see line 64 of the main file.
 
## Pre-requisite (Tested on the folowing versions):
 - Mlagents: 0.16.1 
 - mlagents-envs: 0.26.0
 - gym_unity: 0.26.0
    
<figure>
 <img src="images/workspace2_updated3.png" width="600""> 
<figure>

<figure>
 <img src="images/training_results_v2.png" width="600""> 
<figure>
 
 
# References
 https://ieeexplore.ieee.org/document/9636175
 '''
 @inproceedings{pore2021safe,
  title={Safe Reinforcement Learning using Formal Verification for Tissue Retraction in Autonomous Robotic-Assisted Surgery},
  author={Pore, Ameya and Corsi, Davide and Marchesini, Enrico and Dall’Alba, Diego and Casals, Alicia and Farinelli, Alessandro and Fiorini, Paolo},
  booktitle={2021 IEEE/RSJ International Conference on Intelligent Robots and Systems (IROS)},
  pages={4025--4031},
  year={2021},
  organization={IEEE}
}
'''
