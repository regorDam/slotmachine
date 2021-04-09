using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{

    [SerializeField] private List<UnityEvent> _events;

    [SerializeField]
    Sprite normal;

    [SerializeField]
    Sprite pressed;

    SpriteRenderer spriteRenderer;

    public bool isEnabled = true;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if(isEnabled)
            spriteRenderer.sprite = pressed;
    }

    void OnMouseUp()
    {
        if (!isEnabled) return;

        isEnabled = false;

        GetComponent<AudioSource>().Play();
        spriteRenderer.sprite = normal;

        foreach (UnityEvent _event in _events)
        {
            _event.Invoke();
        }
    }
}
