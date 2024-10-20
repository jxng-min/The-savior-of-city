using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreCtrl : MonoBehaviour
{
    public static int m_kill_count = 0;

    public TMP_Text m_score_text;

    void Update()
    {
        m_score_text.text = "킬 : " + m_kill_count.ToString("0000");
    }

}
