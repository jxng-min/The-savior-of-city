using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Singleton;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource m_effect;

    private AudioClip m_skill_1_effect;
    private AudioClip m_skill_2_effect;
    private AudioClip m_skill_3_effect;

    private AudioClip m_player_damage_effect;
    private AudioClip m_player_dead_effect;
    private AudioClip m_player_clear_effect;

    private AudioClip m_slime_dead_effect;
    private AudioClip m_archer_dead_effect;
    private AudioClip m_knight_dead_effect;

    private void Start()
    {
        m_skill_1_effect = Resources.Load<AudioClip>("07. Sounds/skill_1");
        m_skill_2_effect = Resources.Load<AudioClip>("07. Sounds/skill_2");
        m_skill_3_effect = Resources.Load<AudioClip>("07. Sounds/skill_3");

        m_player_damage_effect = Resources.Load<AudioClip>("07. Sounds/player_damage");
        m_player_dead_effect = Resources.Load<AudioClip>("07. Sounds/player_dead");
        m_player_clear_effect = Resources.Load<AudioClip>("07. Sounds/player_clear");

        m_slime_dead_effect = Resources.Load<AudioClip>("07. Sounds/slime_dead");
        m_archer_dead_effect = Resources.Load<AudioClip>("07. Sounds/archer_dead");
        m_knight_dead_effect = Resources.Load<AudioClip>("07. Sounds/knight_dead");
    }

    public void ButtonSkill1()
    {
        m_effect.PlayOneShot(m_skill_1_effect);
    }

    public void ButtonSkill2()
    {
        m_effect.PlayOneShot(m_skill_2_effect);
    }

    public void ButtonSkill3()
    {
        m_effect.PlayOneShot(m_skill_3_effect);
    }

    public void PlayerDamage()
    {
        m_effect.PlayOneShot(m_player_damage_effect);
    }

    public void PlayerDead()
    {
        m_effect.PlayOneShot(m_player_dead_effect);
    }

    public void SlimeDead()
    {
        m_effect.PlayOneShot(m_slime_dead_effect);
    }

    public void ArcherDead()
    {
        m_effect.PlayOneShot(m_archer_dead_effect);
    }

    public void KngihtDead()
    {
        m_effect.PlayOneShot(m_knight_dead_effect);
    }
}