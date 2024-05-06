using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public enum State
    {
        Playing,
        Dead,
        Clear
    }

    static public State player_state;

    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_sprite_renderer;

    private float m_axis_h;              // 수평 축 입력
    private float m_move_speed = 3.0f;   // 이동 속도
    private float m_jump_speed = 5.0f;   // 점프력

    // 애니메이션 관리를 위한 변수들
    //==========================================
    [SerializeField]
    private LayerMask m_ground_layer;     // GROUND 레이어

    private bool m_do_jump = false;        // 점프 개시 플래그
    private bool m_on_ground = false;      // 지면 충돌 플래그

    private Animator m_animator;
    private string m_idle_anime = "Idle";

    private string m_current_anime;
    private string m_before_anime;
    //==========================================

    private int m_player_attack = 10;

        // 체력바 설정 변수
    //===========================================
    [SerializeField]
    private Slider m_hp_bar_slider;
    private int m_player_hp = 100;
    //===========================================

    // Q 스킬 구현
    //==========================================
    private float m_current_attack1_time;           // 현재 쿨 타임
    private float m_attack1_cool_time = 0.5f;       // 고정 쿨 타임
    public Transform m_attack1_pos;                 // 히트 박스의 위치
    public Vector2 m_attack1_box_size;              // 히트 박스의 크기
    //==========================================

    // W 스킬 구현
    //==========================================
    private float m_current_attack2_time;           // 현재 쿨 타임
    private float m_attack2_cool_time = 1.0f;       // 고정 쿨 타임
    public Transform m_attack2_pos;                 // 히트 박스의 위치
    public Vector2 m_attack2_box_size;              // 히트 박스의 크기
    //==========================================

    // E 스킬 구현
    //==========================================
    private float m_current_attack3_time;           // 현재 쿨 타임
    private float m_attack3_cool_time = 1.0f;       // 고정 쿨 타임
    public Transform m_attack3_pos;                 // 히트 박스의 위치
    public Vector2 m_attack3_box_size;              // 히트 박스의 크기
    //==========================================

    void Start()
    {
        m_rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        m_sprite_renderer = this.gameObject.GetComponent<SpriteRenderer>();
        m_animator = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Move();

        if(Input.GetButtonDown("Jump"))
            Jump();

        Attack1();
        Attack2();
        Attack3();
        SetHpBarState();

        if(m_player_hp <= 0 && player_state != State.Dead)
            Dead();
    }

    void FixedUpdate()
    {
        CheckOnGround();

        if(m_on_ground || (m_axis_h != 0))
        {
            m_rigidbody.velocity = new Vector2(m_move_speed * m_axis_h
                                                , m_rigidbody.velocity.y);
        }
        
        if(m_on_ground && m_do_jump)
        {
            Vector2 jump_power = new Vector2(0, m_jump_speed);

            m_rigidbody.AddForce(jump_power, ForceMode2D.Impulse);
            m_do_jump = false;
        }

        SetPlayerAction();
    }

    void Move()
    {
        m_axis_h = Input.GetAxisRaw("Horizontal");

        if(m_axis_h > 0.0f)
        {
            this.gameObject.transform.localScale = new Vector2(5, 5);
        }
        else if(m_axis_h < 0.0f)
        {
            this.gameObject.transform.localScale = new Vector2(-5, 5);
        }
    }

    void Jump()
    {
        m_do_jump = true;
    }

    void CheckOnGround()
    {
        m_on_ground = Physics2D.Linecast(this.gameObject.transform.position,
                                        this.gameObject.transform.position - (transform.up * 0.1f),
                                        m_ground_layer);
    }

    void SetPlayerAction()
    {
        if(m_on_ground)
        {
            if(m_axis_h == 0)
                m_animator.SetBool("IsMove", false);
            else
                m_animator.SetBool("IsMove", true);
        }
        else
        {
            m_current_anime = m_idle_anime;
        }

        if(m_current_anime != m_before_anime)
        {
            m_before_anime = m_current_anime;
            m_animator.Play(m_current_anime);
        }
        
    }

    void Attack1()
    {
        if(m_current_attack1_time <= 0.0f)
        {
            if(Input.GetKey(KeyCode.Q))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(m_attack1_pos.position, m_attack1_box_size, 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    if(collider.gameObject.layer != 8)
                    {
                        if(collider.CompareTag("SLIME"))
                            collider.GetComponent<SlimeCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                        else if(collider.CompareTag("ARCHER"))
                            collider.GetComponent<ArcherCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                        else if(collider.CompareTag("KNIGHT"))
                            collider.GetComponent<KnightCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                    }
                   
                }

                m_animator.SetTrigger("Attack1");
                m_current_attack1_time = m_attack1_cool_time;
            }
        }
        else
        {
            m_current_attack1_time -= Time.deltaTime;
        }
    }

    void Attack2()
    {
        if(m_current_attack2_time <= 0.0f)
        {
            if(Input.GetKey(KeyCode.W))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(m_attack2_pos.position, m_attack2_box_size, 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    if(collider.gameObject.layer != 8)
                    {
                        if(collider.CompareTag("SLIME"))
                            collider.GetComponent<SlimeCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                        else if(collider.CompareTag("ARCHER"))
                            collider.GetComponent<ArcherCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                        else if(collider.CompareTag("KNIGHT"))
                            collider.GetComponent<KnightCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                    }
                    
                }

                m_animator.SetTrigger("Attack2");
                m_current_attack2_time = m_attack2_cool_time;
            }
        }
        else
        {
            m_current_attack2_time -= Time.deltaTime;
        }
    }

    void Attack3()
    {
        if(m_current_attack3_time <= 0.0f)
        {
            if(Input.GetKey(KeyCode.E))
            {
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(m_attack3_pos.position, m_attack3_box_size, 0);
                foreach(Collider2D collider in collider2Ds)
                {
                    if(collider.gameObject.layer != 8)
                    {
                        if(collider.CompareTag("SLIME"))
                            collider.GetComponent<SlimeCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                        else if(collider.CompareTag("ARCHER"))
                            collider.GetComponent<ArcherCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                        else if(collider.CompareTag("KNIGHT"))
                            collider.GetComponent<KnightCtrl>().TakeDamage(m_player_attack + Random.Range(5, 10));
                    }
                }

                m_animator.SetTrigger("Attack3");
                m_current_attack3_time = m_attack3_cool_time;
            }
        }
        else
        {
            m_current_attack3_time -= Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(m_attack1_pos.position, m_attack1_box_size);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_attack2_pos.position, m_attack2_box_size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m_attack3_pos.position, m_attack3_box_size);
    }

    public void TakeDamage(Vector2 target_pos, int damage)
    {
        this.gameObject.layer = 10;

        m_animator.SetTrigger("Damage");
        m_sprite_renderer.color = new Color(1, 1, 1, 0.4f);

        m_player_hp -= damage;
    
        Invoke("OffDamage", 2.0f);
    }

    public void HealPlayer(int damage)
    {
        if(m_player_hp >= 100)
            m_player_hp = 100;
        else
            m_player_hp += damage;
    }

    void OffDamage()
    {
        this.gameObject.layer = 6;
        m_sprite_renderer.color = new Color(1, 1, 1, 1);
    }

    void SetHpBarState()
    {
        float target_hp_value = (float)m_player_hp / 100.0f;
        m_hp_bar_slider.value = Mathf.Lerp(m_hp_bar_slider.value, target_hp_value, Time.deltaTime * 5.0f);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.collider.gameObject.layer != 8)
        {
            if(coll.collider.CompareTag("SLIME") || coll.collider.CompareTag("ARCHER"))
            {
                TakeDamage(coll.transform.position, 5 + Random.Range(0, 5));
            }
            else if(coll.collider.CompareTag("ARROW"))
            {
                TakeDamage(coll.transform.position, 10 + Random.Range(0, 5));            
            }
            else if(coll.collider.CompareTag("KNIGHT"))
            {
                TakeDamage(coll.transform.position, 10 + Random.Range(0, 5));
            }
        }
    }

    void Dead()
    {
        m_animator.SetTrigger("Dead");
        player_state = State.Dead;
    }
}
