using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_dead_panel;
    [SerializeField]
    private GameObject m_clear_panel;

    private bool m_is_sound_played = false;

    void Start()
    {
        m_is_sound_played = false;
        KillCounterCtrl.m_kill_count = 0;
        PlayerCtrl.player_state = PlayerCtrl.State.Playing;
        m_dead_panel.gameObject.SetActive(false);
        m_clear_panel.gameObject.SetActive(false);
    }

    void Update()
    {
        if(PlayerCtrl.player_state == PlayerCtrl.State.Dead)
        {
            if(!m_is_sound_played)
            {
                m_is_sound_played = true;
                SoundManager.instance.PlaySE("GameOver");
            }
            m_dead_panel.gameObject.SetActive(true);
        }
        else if(PlayerCtrl.player_state == PlayerCtrl.State.Clear)
        {
            if(!m_is_sound_played)
            {
                m_is_sound_played = true;
                SoundManager.instance.PlaySE("GameClear");
            }
            m_clear_panel.gameObject.SetActive(true);
        }
    }
}
