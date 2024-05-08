using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SECtrl : MonoBehaviour
{
    #region  singleton
    static public SECtrl m_instance;

    private void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion singleton
}