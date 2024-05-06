using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCtrl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_text;

    void Update()
    {
        int score = 0;
        if(PlayerCtrl.player_state == PlayerCtrl.State.Clear)
            score = KillCounterCtrl.m_kill_count * 10 + 1000;
        else
            score = KillCounterCtrl.m_kill_count * 10;

        m_text.text = "Score : " + score.ToString();
    }

}
