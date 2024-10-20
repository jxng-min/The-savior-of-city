using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using _Singleton;
using _EventBus;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        SETTING, PLAYING, DEAD, CLEAR,
    }

    public GameState State { get; set; }
    public GameObject m_state_canvas;
    public GameObject m_dead_panel;
    public GameObject m_clear_panel;

    [SerializeField] 
    private JoyStickValue m_joy_value;

    private void OnEnable()
    {
        GameEventBus.Subscribe(GameEventType.SETTING, GameManager.Instance.Setting);
        GameEventBus.Subscribe(GameEventType.PLAYING, GameManager.Instance.Playing);
        GameEventBus.Subscribe(GameEventType.DEAD, GameManager.Instance.Dead);
        GameEventBus.Subscribe(GameEventType.CLEAR, GameManager.Instance.Clear);
    }

    private void OnDisable()
    {
        GameEventBus.Unsubscribe(GameEventType.SETTING, GameManager.Instance.Setting);
        GameEventBus.Unsubscribe(GameEventType.PLAYING, GameManager.Instance.Playing);
        GameEventBus.Unsubscribe(GameEventType.DEAD, GameManager.Instance.Dead);
        GameEventBus.Unsubscribe(GameEventType.CLEAR, GameManager.Instance.Clear);
    }

    private void Start()
    {
        DontDestroyOnLoad(m_state_canvas);
        GameEventBus.Publish(GameEventType.SETTING);
    }

    public void Setting()
    {
        State = GameState.SETTING;

        m_dead_panel.SetActive(false);
        m_clear_panel.SetActive(false);
    }

    public void Playing()
    {
        State = GameState.PLAYING;

        m_dead_panel.SetActive(false);
        m_clear_panel.SetActive(false);
    }

    public void Dead()
    {
        State = GameState.DEAD;

        m_dead_panel.SetActive(true);
        m_clear_panel.SetActive(false);
    }

    public void Clear()
    {
        State = GameState.CLEAR;

        m_dead_panel.SetActive(false);
        m_clear_panel.SetActive(true);
    }
}
