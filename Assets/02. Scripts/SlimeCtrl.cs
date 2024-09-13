using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeCtrl : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_sprite_renderer;
    private int m_next_move;
    private float m_move_speed = 1.5f;
    private Animator m_animator;
    private BoxCollider2D m_collider;

    private int m_hp = 50;
    private bool m_is_dead = false;

    // 체력바 설정 변수
    //===========================================
    [SerializeField]
    private GameObject m_canvas;
    [SerializeField]
    private GameObject m_hp_bar;
    private RectTransform m_hp_bar_rt;
    private Slider m_hp_bar_slider;
    private float m_hp_bar_height = 1.0f;
    //===========================================

    // 점프 관련 변수
    //===========================================
    private float m_jump_speed = 2.0f;
    [SerializeField]
    private LayerMask m_ground_layer;
    private bool m_do_jump = false;
    private bool m_on_ground = false;
    //===========================================

    private GameObject m_player;

    private float m_knockback_speed = 1.0f;

    void Awake()
    {
        m_rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        m_sprite_renderer = this.gameObject.GetComponent<SpriteRenderer>();
        m_animator = this.gameObject.GetComponent<Animator>();
        m_player = GameObject.FindGameObjectWithTag("Player");

        m_sprite_renderer.color = new Color(1, 1, 1, 0.4f);

        Invoke("SetEnemy", 2.0f);
        Invoke("SetNextMove", 2.0f);
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
        if(m_hp <= 0 && !m_is_dead)
            Dead();

        if(m_hp != 50)
            m_hp_bar_rt.gameObject.SetActive(true);
        SetHpBarPosition();
        SetHpBarState();
    }

    void FixedUpdate()
    {
        CheckOnGround();
        Move();
    }

    void SetEnemy()
    {
        this.gameObject.tag = "SLIME";
        this.gameObject.layer = 7;
        m_sprite_renderer.color = new Color(1, 1, 1, 1);
    }

    // 적의 움직임을 담당하는 함수
    void Move()
    {
        if(-1 <= m_next_move && m_next_move <= 1)
            m_rigidbody.velocity = new Vector2(m_next_move * m_move_speed, m_rigidbody.velocity.y);
        else
        {
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
            m_do_jump = true;
            if(m_on_ground && m_do_jump)
                Jump();
        }
        
        if(m_next_move < 0.0f)
            m_rigidbody.transform.localScale = new Vector2(-5, 5);
        else if(m_next_move > 0.0f)
            m_rigidbody.transform.localScale = new Vector2(5, 5);
    }

    // 적의 다음 움직임을 정하는 함수
    void SetNextMove()
    {
        m_next_move = Random.Range(-1, 3);

        float think_next_move_time = Random.Range(1.0f, 3.0f);

        Invoke("SetNextMove", think_next_move_time);
    }

    // 적이 땅 위에 존재하는지 확인하는 함수
    void CheckOnGround()
    {
        m_on_ground = Physics2D.Linecast(this.gameObject.transform.position,
                                        this.gameObject.transform.position - (transform.up * 0.1f),
                                        m_ground_layer);
    }

    // 적의 점프를 담당하는 함수
    void Jump()
    {
        Vector2 jump_power = new Vector2(0, m_jump_speed);
        m_rigidbody.AddForce(jump_power, ForceMode2D.Impulse);
        m_do_jump = false;
    }

    void KnockBack()
    {
        Vector2 knockback_power = Vector2.zero;
        if(this.gameObject.transform.localScale.x < 0.0f)
            knockback_power = new Vector2(m_knockback_speed * 3, 1);
        else
            knockback_power = new Vector2(-m_knockback_speed * 3, 1);
        m_rigidbody.AddForce(knockback_power, ForceMode2D.Impulse);
    }

    // 플레이어의 공격에 데미지를 받는 경우를 처리하는 함수
    public void TakeDamage(int damage)
    {
        m_hp -= damage;
        m_animator.SetTrigger("Damage");
        KnockBack();
        CancelInvoke();
        Invoke("SetNextMove", 2.0f);
    }

    // 적이 죽었을 경우를 처리하는 함수
    void Dead()
    {
        this.gameObject.layer = 8;

        m_rigidbody.velocity = new Vector2(0, 0);
        CancelInvoke();

        m_is_dead = true;
        m_animator.SetTrigger("Dead");
        m_player.GetComponent<PlayerCtrl>().HealPlayer(1);
        SoundManager.instance.PlaySE("Slime");

        StartCoroutine(DestroyObj(1.2f));

        KillCounterCtrl.m_kill_count++;
        Debug.Log("슬라임 파괴됨");
    }

    // 게임 오브젝트 파괴 래퍼 함수
    IEnumerator DestroyObj(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    // 체력바 위치를 정하는 함수
    void SetHpBarPosition()
    {
        Vector3 hp_bar_pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + m_hp_bar_height, 0));
        m_hp_bar_rt.position = hp_bar_pos;
    }

    void SetHpBarState()
    {
        float target_hp_value = (float)m_hp / 50.0f;
        m_hp_bar_slider.value = Mathf.Lerp(m_hp_bar_slider.value, target_hp_value, Time.deltaTime * 5.0f);
    }
}
