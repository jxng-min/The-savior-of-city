using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    public string m_scene_name;

    public void LoadScene()
    {
        SceneManager.LoadScene(m_scene_name);
    }
}
