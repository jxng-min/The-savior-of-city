using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightCtrl : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_sprite_renderer;
    private Animator m_animator;

    public int m_next_move;
    private float m_move_speed = 1.0f;
    public int m_hp = 300;
    public int m_attack = 15;
    private bool m_is_dead = false;
    private bool m_is_wake_up = false;

    [SerializeField]
    private GameObject m_canvas;
    [SerializeField]
    private GameObject m_hp_bar;
    private RectTransform m_hp_bar_rt;
    private Slider m_hp_bar_slider;
    private float m_hp_bar_height = 1.0f;

    private float m_current_attack_time;           // 현재 쿨 타임
    private float m_attack_cool_time = 2.0f;       // 고정 쿨 타임
    public Transform m_attack_pos;                 // 히트 박스의 위치
    public Vector2 m_attack_box_size;              // 히트 박스의 크기

    void Awake()
    {
        m_rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        m_sprite_renderer = this.gameObject.GetComponent<SpriteRenderer>();
        m_animator = this.gameObject.GetComponent<Animator>();

        m_sprite_renderer.color = new Color(1, 1, 1, 0.4f);

        Invoke("SetEnemy", 2.0f);
    }

    void Start()
    {
        m_hp_bar_rt = Instantiate(m_hp_bar, m_canvas.transform).GetComponent<RectTransform>();
        m_hp_bar_rt.gameObject.SetActive(false);
        m_hp_bar_slider = m_hp_bar_rt.gameObject.GetComponent<Slider>();
        m_hp_bar_slider.value = 1;
    }

    void Update()
    {
        if(m_hp != 300 && !m_is_wake_up)
        {
            m_is_wake_up = true;
            Invoke("SetNextMove", 2.0f);
        }

        if(m_hp <= 0 && !m_is_dead)
            Dead();

        if(m_hp != 300)
            m_hp_bar_rt.gameObject.SetActive(true);
        SetHpBarPosition();
        SetHpBarState();

        m_current_attack_time -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Move();
    }

    void SetEnemy()
    {
        this.gameObject.tag = "KNIGHT";
        this.gameObject.layer = 7;
        m_sprite_renderer.color = new Color(1, 1, 1, 1);
    }

    void Move()
    {
        if(m_is_wake_up)
        {
            if(-1 <= m_next_move && m_next_move <= 1)
            {
                m_rigidbody.velocity = new Vector2(m_next_move * m_move_speed, m_rigidbody.velocity.y);
                if(m_next_move != 0)
                    m_animator.SetBool("IsMove", true);
                else
                    m_animator.SetBool("IsMove", false);
            }
            else
            {
                m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
                Attack();
            }

            if(m_next_move < 0.0f)
                m_rigidbody.transform.localScale = new Vector2(-5, 5);
            else if(m_next_move > 0.0f)
                m_rigidbody.transform.localScale = new Vector2(5, 5);
        }
    }

    void SetNextMove()
    {
        m_next_move = Random.Range(-1, 3);
        float think_next_move_time = Random.Range(1.0f, 3.0f);
        Invoke("SetNextMove", think_next_move_time);
    }

    public void TakeDamage(int damage)
    {
        m_hp -= damage;
        if(m_is_wake_up)
            m_animator.SetTrigger("Damage");
        else
            m_animator.SetTrigger("FirstDamage");
        CancelInvoke();
        Invoke("SetNextMove", 2.0f);
    }

    void Dead()
    {
        this.gameObject.layer = 8;

        m_rigidbody.velocity = new Vector2(0, 0);
        CancelInvoke();

        m_is_dead = true;
        m_animator.SetTrigger("Dead");
        SoundManager.Instance.KngihtDead();

        StartCoroutine(DestroyObj(1.2f));

        ScoreCtrl.m_kill_count++;
        GameManager.Instance.State = GameManager.GameState.CLEAR;
    }

    IEnumerator DestroyObj(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    void SetHpBarPosition()
    {
        Vector3 hp_bar_pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + m_hp_bar_height, 0));
        m_hp_bar_rt.position = hp_bar_pos;
    }

    void SetHpBarState()
    {
        float target_hp_value = (float)m_hp / 300.0f;
        m_hp_bar_slider.value = Mathf.Lerp(m_hp_bar_slider.value, target_hp_value, Time.deltaTime * 5.0f);
    }

    void Attack()
    {
        if(m_current_attack_time <= 0.0f)
        {
            SoundManager.Instance.KngihtDead();
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(m_attack_pos.position, m_attack_box_size, 0);
            foreach(Collider2D collider in collider2Ds)
            {
                if(collider.CompareTag("Player"))
                {
                    // collider.GetComponent<PlayerCtrl>().TakeDamage(transform.position, m_attack + Random.Range(5, 10));
                }
            }

            m_animator.SetTrigger("Attack");
            m_current_attack_time = m_attack_cool_time;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(m_attack_pos.position, m_attack_box_size);
    }
}
