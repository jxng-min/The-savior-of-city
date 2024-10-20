using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarCtrl : MonoBehaviour
{
    public PlayerCtrl m_player_ctrl;
    public Slider m_hp_bar_slider;

    private void Update()
    {
        SetHpBar();
    }

    private void SetHpBar()
    {
        float target_hp_value = (float)m_player_ctrl.m_player_hp / 100.0f;
        m_hp_bar_slider.value = Mathf.Lerp(m_hp_bar_slider.value, target_hp_value, Time.deltaTime);
    }
}
