using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    [SerializeField]
    private string m_scene_name;

    public void LoadScene()
    {
        SceneManager.LoadScene(m_scene_name);
    }
}
