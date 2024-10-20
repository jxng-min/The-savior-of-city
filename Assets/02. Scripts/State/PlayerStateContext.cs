using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _State
{
    public class PlayerStateContext
    {
        public IPlayerState CurrentState
        {
            get; set;
        }

        private readonly PlayerCtrl m_player_ctrl;

        public PlayerStateContext(PlayerCtrl player_ctrl)
        {
            m_player_ctrl = player_ctrl;
        }

        public void Transition()
        {
            CurrentState.Handle(m_player_ctrl);
        }

        public void Transition(IPlayerState state)
        {
            CurrentState = state;
            CurrentState.Handle(m_player_ctrl);
        }
    }
}
