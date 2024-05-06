using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCtrl : MonoBehaviour
{
    public Transform m_spawn_area_position;
    public Vector2 m_spawn_area_size;
    public GameObject[] m_enemy_types;
    private Vector2[] enemy_counts = {new Vector2(2, 5), new Vector2(5, 10), new Vector2(7, 12), new Vector2(0, 0)};
    static public int m_stage_level = 1; 

    
    void Start()
    {
        Invoke("SpawnEnemy", 10);
        Invoke("SetStageLevel", 300.0f);
    }

    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(m_spawn_area_position.position, m_spawn_area_size);
    }

    void SpawnEnemy()
    {
        if(m_stage_level <= 2)
        {
            Vector2 current_range = enemy_counts[m_stage_level - 1];
            int enemy_count = Random.Range((int)current_range.x, (int)current_range.y);

            for(int i = 0; i < enemy_count; i++)
            {
                Vector2 spawn_point = new Vector2(Random.Range(-10.5f, 10.5f), -5);
                int enemy_type = Random.Range(0, m_stage_level);
                Instantiate(m_enemy_types[enemy_type], spawn_point, Quaternion.identity);
            }

            Invoke("SpawnEnemy", 10);
        }
        else
            Instantiate(m_enemy_types[m_stage_level - 1], new Vector2(0, -5), Quaternion.identity);
    }
    
    void SetStageLevel()
    {
        m_stage_level++;
        if(m_stage_level <= 2)
            Invoke("SetStageLevel", 300.0f);
    }
}
