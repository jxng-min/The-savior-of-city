using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _State
{
    public class PlayerDamageState : MonoBehaviour, IPlayerState
    {
        private PlayerCtrl m_player_ctrl;

        public void Handle(PlayerCtrl player_ctrl)
        {
            if(!m_player_ctrl)
                m_player_ctrl = player_ctrl;

            TakeDamage();
        }

        private void TakeDamage()
        {
            this.gameObject.layer = 10;

            m_player_ctrl.m_animator.SetTrigger("Damage");
            m_player_ctrl.m_sprite_renderer.color = new Color(1, 1, 1, 0.4f);
        
            Invoke("OffDamage", 2.0f);
        }

        private void OffDamage()
        {
            this.gameObject.layer = 6;
            m_player_ctrl.m_sprite_renderer.color = new Color(1, 1, 1, 1);
        }
    }
}