using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _State
{
    public class PlayerMoveState : MonoBehaviour, IPlayerState
    {
        private PlayerCtrl m_player_ctrl;

        public void Handle(PlayerCtrl player_ctrl)
        {
            if(!m_player_ctrl)
                m_player_ctrl = player_ctrl;

            m_player_ctrl.m_animator.SetBool("IsMove", true);
            SetPlayerDirection();
        }

        private void SetPlayerDirection()
        {
            if(m_player_ctrl.m_value.m_joy_touch.x > 0f)
                m_player_ctrl.PlayerDirection = Quaternion.Euler(0f, 0f, 0f);
            else if(m_player_ctrl.m_value.m_joy_touch.x < 0f)
                m_player_ctrl.PlayerDirection = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
