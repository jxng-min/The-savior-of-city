using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    private float m_speed;
    private float m_distance = 0.5f;
    [SerializeField]
    private LayerMask m_is_layer;
    void Start()
    {
        Invoke("DestroyObj", 2.0f);
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, m_distance, m_is_layer);
        if(ray.collider != null)
        {
            if(ray.collider.CompareTag("Player"))
                Destroy(gameObject);
        }
        
        if(transform.rotation.y == 0)
            transform.Translate(transform.right * m_speed * Time.deltaTime);
        else
            transform.Translate(transform.right * - 1 * m_speed * Time.deltaTime);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
