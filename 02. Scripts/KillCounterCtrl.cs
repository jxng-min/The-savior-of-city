using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillCounterCtrl : MonoBehaviour
{
    static public int m_kill_count = 0;
    [SerializeField]
    private TMP_Text m_text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetKillCounter();
    }

    void SetKillCounter()
    {
        m_text.text = "Kill : " + m_kill_count.ToString("0000");
    }
}
