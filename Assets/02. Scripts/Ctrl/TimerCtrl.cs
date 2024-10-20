using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TimerCtrl : MonoBehaviour
{
    public TMP_Text m_text;
    private float m_current_time = 0.0f;
    
    static public int m_minute;
    static public int m_second;

    void Update()
    {
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        m_current_time += Time.deltaTime;

        m_minute = (int)m_current_time / 60;
        m_second = (int)m_current_time % 60;

        m_text.text = "플레이 타임: " + m_minute.ToString("00") + ":" + m_second.ToString("00");

        if(m_current_time % 300 == 0)
            SpawnCtrl.m_stage_level++;
    
        yield return null;
    }
}
