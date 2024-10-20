using System.Collections;
using System.Collections.Generic;
using _EventBus;
using UnityEngine;

namespace _State
{
    public class PlayerDeadState : MonoBehaviour, IPlayerState
    {
        private PlayerCtrl m_player_ctrl;

        public void Handle(PlayerCtrl player_ctrl)
        {
            if(!m_player_ctrl)
                m_player_ctrl = player_ctrl;

            GameManager.Instance.State = GameManager.GameState.DEAD;
            GameEventBus.Publish(GameEventType.DEAD);
        }
    }
}
