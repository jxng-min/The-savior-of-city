using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform m_rect;
    [SerializeField] private RectTransform m_handle;
    private Vector2 m_touch = Vector2.zero;
    private float m_width_half;
    [SerializeField] JoyStickValue m_value;

    private void Start()
    {
        m_rect = GetComponent<RectTransform>();
        m_width_half = m_rect.sizeDelta.x * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rect, eventData.position, eventData.pressEventCamera, out localPoint);
        m_touch = localPoint / m_width_half;

        if(m_touch.magnitude > 1)
            m_touch = m_touch.normalized;
        m_value.m_joy_touch = m_touch;

        m_handle.anchoredPosition = m_touch * m_width_half;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_value.m_joy_touch = Vector2.zero;
        m_handle.anchoredPosition = Vector2.zero;
    }
}
