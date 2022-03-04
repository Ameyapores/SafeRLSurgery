"""Launch file for the testing of the Da Vinci scene.
"""

import os
import sys
import time

import yaml
import numpy as np

from mlagents_envs.environment import UnityEnvironment
from gym_unity.envs import UnityToGymWrapper
from utils.deepnetwork import DeepNetwork

if (sys.version_info[0] < 3):
    raise Exception("ERROR: ML-Agents Toolkit 6 requires Python > 3.6")

with open('config2.yml', 'r') as ymlfile:
    cfg = yaml.load(ymlfile, Loader=yaml.FullLoader)
    seed = cfg['setup']['seed']
    ymlfile.close()

if not cfg['setup']['use_gpu']:
    os.environ['CUDA_VISIBLE_DEVICES'] = '-1'

os.environ['PYTHONHASHSEED'] = str(seed)

def round_obs(obs):
    return np.around(obs, decimals=4)

def get_action(model, state, std=0.0):
    """Get the action to perform

    Args:
        model (tf_model): trained model
        state (list): agent current state
        std (float): std for the Gaussian in cc

    Returns:
        discrete_action (list): sampled action to perform, integers
    """

    mu = model(np.array([state])).numpy()[0]
    action = np.random.normal(loc=mu, scale=std)

    discrete_action = []
    for i in range(len(action)):
        if action[i] <= -0.3:
            discrete_action.append(0)    
        if action[i] > -0.3 and action[i] < 0.3:
            discrete_action.append(1) 
        if action[i] >= 0.3:
            discrete_action.append(2)  

    return discrete_action


def main(cfg):
    # env = UnityToGymWrapper(unity_env=cfg['train']['name'])
    # env = UnityToGymWrapper(unity_env=cfg['train']['name'])
    unity_env = UnityEnvironment(None)
    # unity_env = UnityEnvironment(cfg['train']['name'])
    env = UnityToGymWrapper(unity_env)
    # env = UnityToGymWrapper(unity_env=None)
    '''
    # Use this one for interacting in the editor
    env = UnityEnv(
        environment_filename=None, 
        worker_id=0
    )
    '''

    model = DeepNetwork.build(env, cfg['agent']['actor'], name='actor')
    path = "results\\IROS_davinci_PPO_MC_discrete_seed2032_2x128.h5"
    model.load_weights(path)

    for _ in range(10):
        state = round_obs(env.reset())

        while True:
            discrete_action = get_action(model, state)

            obs_state, _, done, _ = env.step([discrete_action])
            state = round_obs(obs_state)

            if done: break  

        time.sleep(2)

if __name__ == "__main__":
    main(cfg)