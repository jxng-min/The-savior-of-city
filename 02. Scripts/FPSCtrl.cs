using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCtrl : MonoBehaviour
{
    public TMP_Text m_fps_text;
    public TMP_Text m_ms_text;

    void Start()
    {
        ShowFPS();
    }

    void ShowFPS()
    {
        float fps = 1.0f / Time.deltaTime;
        float ms = Time.deltaTime * 1000.0f;

        m_fps_text.text = "FPS : " + fps.ToString("000");
        m_ms_text.text = "ms : " + ms.ToString("0");

        Invoke("ShowFPS", 1.0f);
    }
}
