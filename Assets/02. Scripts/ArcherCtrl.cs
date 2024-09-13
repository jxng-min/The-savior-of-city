using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherCtrl : MonoBehaviour
{
    private Rigidbody2D m_rigidbody;
    private SpriteRenderer m_sprite_renderer;
    private Animator m_animator;
    public int m_next_move;
    private float m_move_speed = 1.0f;
    public int m_hp = 40;
    private bool m_is_dead = false;

    private float m_jump_speed = 2.0f;
    [SerializeField]
    private LayerMask m_ground_layer;
    bool m_do_jump = false;
    bool m_on_ground = false;

    [SerializeField]
    private GameObject m_canvas;
    [SerializeField]
    private GameObject m_hp_bar;
    private RectTransform m_hp_bar_rt;
    private Slider m_hp_bar_slider;
    private float m_hp_bar_height = 1.0f;

    private GameObject m_player;

    [SerializeField]
    private GameObject m_arrow;

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

        if(m_hp != 40)
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
        this.gameObject.tag = "ARCHER";
        this.gameObject.layer = 7;
        m_sprite_renderer.color = new Color(1, 1, 1, 1);
        Invoke("ShootArrow", Random.Range(3, 5));
    }

    void Move()
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
            m_do_jump = true;
            if(m_on_ground && m_do_jump)
                Jump();
        }

        if(m_next_move < 0.0f)
            m_rigidbody.transform.localScale = new Vector2(-5, 5);
        else if(m_next_move > 0.0f)
            m_rigidbody.transform.localScale = new Vector2(5, 5);
    }

    void SetNextMove()
    {
        m_next_move = Random.Range(-1, 3);
        float think_next_move_time = Random.Range(1.0f, 3.0f);
        Invoke("SetNextMove", think_next_move_time);
    }

    void Jump()
    {
        Vector2 jump_power = new Vector2(0, m_jump_speed);
        m_rigidbody.AddForce(jump_power, ForceMode2D.Impulse);
        m_do_jump = false;
    }

    void CheckOnGround()
    {
        m_on_ground = Physics2D.Linecast(this.gameObject.transform.position,
                                        this.gameObject.transform.position - (transform.up * 0.1f),
                                        m_ground_layer);
    }

    public void TakeDamage(int damage)
    {
        m_hp -= damage;
        m_animator.SetTrigger("Damage");
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
        m_player.GetComponent<PlayerCtrl>().HealPlayer(1);
        SoundManager.instance.PlaySE("Archer");

        StartCoroutine(DestroyObj(1.2f));

        KillCounterCtrl.m_kill_count++;
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
        float target_hp_value = (float)m_hp / 50.0f;
        m_hp_bar_slider.value = Mathf.Lerp(m_hp_bar_slider.value, target_hp_value, Time.deltaTime * 5.0f);
    }
    
    void OnDrawGizmos()
    {
    Gizmos.color = Color.blue;

    Vector3 direction = new Vector3(0, 0, 0);
    if (m_next_move < 0f)
        direction = Vector3.left;
    else if(m_next_move > 0f)
        direction = Vector3.right;
    
    Gizmos.DrawLine(transform.position + new Vector3(0, 0.5f, 0)
    , transform.position + direction + new Vector3(0, 0.5f, 0));
    }

void ShootArrow()
{
    GameObject arrow;

    arrow = Instantiate(m_arrow, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    if(m_next_move < 0f)
    {
        arrow.transform.localScale = new Vector2(-5, 5);
        arrow.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 7, ForceMode2D.Impulse); // 왼쪽 방향으로 힘을 가합니다.
    }
    else if(m_next_move > 0f)
    {
        arrow.transform.localScale = new Vector2(5, 5);
        arrow.GetComponent<Rigidbody2D>().AddForce(Vector2.right, ForceMode2D.Impulse); // 오른쪽 방향으로 힘을 가합니다.
    }

    Invoke("ShootArrow", Random.Range(3, 5));
}
}
