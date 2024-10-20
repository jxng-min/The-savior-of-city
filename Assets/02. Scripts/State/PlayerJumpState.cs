using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _State
{
    public class PlayerJumpState : MonoBehaviour, IPlayerState
    {
        private PlayerCtrl m_player_ctrl;

        public void Handle(PlayerCtrl player_ctrl)
        {
            if(!m_player_ctrl)
                m_player_ctrl = player_ctrl;
            
            SetJumpFlag();
            JumpAction();
        }

        private void SetJumpFlag()
        {
            m_player_ctrl.m_do_jump = true;
        }

        private void JumpAction()
        {
            if(m_player_ctrl.m_on_ground && m_player_ctrl.m_do_jump)
            {
                Vector2 jump_power = new Vector2(0, m_player_ctrl.m_jump_speed);

                m_player_ctrl.m_rigidbody.AddForce(jump_power, ForceMode2D.Impulse);
                m_player_ctrl.m_do_jump = false;
            }
        }
    }
}

