using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using _State;
using _EventBus;

public class PlayerCtrl : MonoBehaviour
{
    public Rigidbody2D m_rigidbody;
    public SpriteRenderer m_sprite_renderer;
    public Animator m_animator;
    public Transform m_transform;


    public float m_move_speed = 3.0f;
    public float m_jump_speed = 5.0f;

    public LayerMask m_ground_layer;
    public bool m_do_jump = false;
    public bool m_on_ground = true;


    public int m_player_attack = 10;
    public int m_player_hp = 100;


    public float[] m_current_attack_time;
    public float[] m_attack_cool_time;
    public Transform[] m_attack_pos;
    public Vector2[] m_attack_box_size;


    private IPlayerState m_stop_state, m_move_state, m_skill1_state, m_skill2_state, m_skill3_state, m_jump_state, m_damage_state, m_dead_state, m_clear_state;
    private PlayerStateContext m_player_state_context;

    public JoyStickValue m_value;
    public float m_joy_value;

    public Quaternion PlayerDirection
    {
        get { return m_transform.rotation; }
        set { m_transform.rotation = value; }
    }

    private void Start()
    {
        GameEventBus.Publish(GameEventType.PLAYING);

        m_player_state_context = new PlayerStateContext(this);

        m_stop_state = gameObject.AddComponent<PlayerStopState>();
        m_move_state = gameObject.AddComponent<PlayerMoveState>();
        m_skill1_state = gameObject.AddComponent<PlayerSkill1State>();
        m_skill2_state = gameObject.AddComponent<PlayerSkill2State>();
        m_skill3_state = gameObject.AddComponent<PlayerSkill3State>();
        m_jump_state = gameObject.AddComponent<PlayerJumpState>();
        m_damage_state = gameObject.AddComponent<PlayerDamageState>();
        m_dead_state = gameObject.AddComponent<PlayerDeadState>();
        m_clear_state = gameObject.AddComponent<PlayerClearState>();

        m_rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        m_sprite_renderer = this.gameObject.GetComponent<SpriteRenderer>();
        m_animator = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
            PlayerJump();

        if(m_player_hp < 0f)
            PlayerDead();

        m_joy_value = 0f;
        if(m_value.m_joy_touch.x > 0f)
            m_joy_value = 1f;
        else if(m_value.m_joy_touch.x < 0f)
            m_joy_value = -1f;

        CheckOnGround();
    }

    private void FixedUpdate()
    {
        if(m_on_ground)
            if(m_value.m_joy_touch.x != 0f)
                PlayerMove();
            else
                PlayerStop();

        m_rigidbody.velocity = new Vector2(m_move_speed * m_joy_value
                                            , m_rigidbody.velocity.y);

        for(int i = 0; i < 3; i++)
            if(0f < m_current_attack_time[i])
                m_current_attack_time[i] -= Time.deltaTime;
    }

    public void PlayerStop()
    {
        m_player_state_context.Transition(m_stop_state);
    }

    public void PlayerMove()
    {
        m_player_state_context.Transition(m_move_state);
    } 

    public void PlayerJump()
    {
        m_player_state_context.Transition(m_jump_state);
    }

    public void PlayerSkill1()
    {
        m_player_state_context.Transition(m_skill1_state);
    }

    public void PlayerSkill2()
    {
        m_player_state_context.Transition(m_skill2_state);
    }

    public void PlayerSkill3()
    {
        m_player_state_context.Transition(m_skill3_state);
    }

    public void PlayerDamage()
    {
        m_player_state_context.Transition(m_damage_state);
    }

    public void PlayerDead()
    {
        m_player_state_context.Transition(m_dead_state);
    }

    private void CheckOnGround()
    {
        m_on_ground = Physics2D.Linecast(transform.position,
                                            transform.position - (transform.up * 0.1f),
                                            m_ground_layer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(m_attack_pos[0].position, m_attack_box_size[0]);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(m_attack_pos[1].position, m_attack_box_size[1]);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(m_attack_pos[2].position, m_attack_box_size[2]);
    }

    public void HealPlayer(int damage)
    {
        if(m_player_hp >= 100)
            m_player_hp = 100;
        else
            m_player_hp += damage;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.collider.gameObject.layer == 7)
        {
            PlayerDamage();

            if(coll.collider.CompareTag("SLIME") || coll.collider.CompareTag("ARCHER"))
                m_player_hp -= (5 + Random.Range(0, 5));

            else if(coll.collider.CompareTag("ARROW"))
                m_player_hp -= (5 + Random.Range(0, 5));     

            else if(coll.collider.CompareTag("KNIGHT"))
                m_player_hp -= (5 + Random.Range(0, 5));
        }
    }
}
