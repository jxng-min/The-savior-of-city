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

    void Start()
    {
        KillCounterCtrl.m_kill_count = 0;
        PlayerCtrl.player_state = PlayerCtrl.State.Playing;
        m_dead_panel.gameObject.SetActive(false);
        m_clear_panel.gameObject.SetActive(false);
    }

    void Update()
    {
        if(PlayerCtrl.player_state == PlayerCtrl.State.Dead)
        {
            SoundManager.instance.PlaySE("GameOver");
            m_dead_panel.gameObject.SetActive(true);
        }
        else if(PlayerCtrl.player_state == PlayerCtrl.State.Clear)
        {
            SoundManager.instance.PlaySE("GameClear");
            m_clear_panel.gameObject.SetActive(true);
        }
    }
}
