using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _EventBus;

namespace _State
{
    public class PlayerClearState : MonoBehaviour, IPlayerState
    {
        private PlayerCtrl m_player_ctrl;

        public void Handle(PlayerCtrl player_ctrl)
        {
            if(!m_player_ctrl)
                m_player_ctrl = player_ctrl;

            GameManager.Instance.State = GameManager.GameState.CLEAR;
            GameEventBus.Publish(GameEventType.CLEAR);
        }
    }
}