using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _State
{
    public class PlayerSkill3State : MonoBehaviour, IPlayerState
    {
        private PlayerCtrl m_player_ctrl;

        public void Handle(PlayerCtrl player_ctrl)
        {
            if(!m_player_ctrl)
                m_player_ctrl = player_ctrl;

            Attack();
        }

        private void Attack()
        {
            if(m_player_ctrl.m_current_attack_time[2] <= 0f)
            {
                SoundManager.Instance.ButtonSkill3();
                m_player_ctrl.m_animator.SetTrigger("Attack3");
            
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(m_player_ctrl.m_attack_pos[2].position, m_player_ctrl.m_attack_box_size[2], 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    if(collider.gameObject.layer != 8)
                    {
                        if(collider.CompareTag("SLIME"))
                            collider.GetComponent<SlimeCtrl>().TakeDamage(m_player_ctrl.m_player_attack + Random.Range(5, 10));
                        else if(collider.CompareTag("ARCHER"))
                            collider.GetComponent<ArcherCtrl>().TakeDamage(m_player_ctrl.m_player_attack + Random.Range(5, 10));
                        else if(collider.CompareTag("KNIGHT"))
                            collider.GetComponent<KnightCtrl>().TakeDamage(m_player_ctrl.m_player_attack + Random.Range(5, 10));
                    }
                }

                m_player_ctrl.m_current_attack_time[2] = m_player_ctrl.m_attack_cool_time[2];
            }
        }
    }
}