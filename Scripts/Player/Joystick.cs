using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField] private RectTransform background;

    private float deadZone = 0;
    private float handleRange = 1;
    private Vector3 input = Vector3.zero;
    private Canvas canvas;

    public float Horizontal { get { return input.x; } }
    public float Vertical { get { return input.y; } }

    // Start is called before the first frame update
    void Start()
    {
        handle = GameObject.Find("JoystickHandle").GetComponent<RectTransform>();
        background = GameObject.Find("JoystickBG").GetComponent<RectTransform>();
        canvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("터치 입력 : " + eventData.position);

        if(GameManager.instance.isPause == false)
        {
            OnDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 radius = background.sizeDelta * 0.5f;
        input = (eventData.position - background.anchoredPosition) / (radius * canvas.scaleFactor);
        HandleInput(input.magnitude, input.normalized);
        handle.anchoredPosition = input * radius * handleRange;
    }

    private void HandleInput(float magnitude, Vector2 normalised)
    {
        if(magnitude > deadZone) // 일정 범위 밖으로 나가지 않도록 함
        {
            if(magnitude > 1)
            {
                input = normalised;
            }
        }
        else
        {
            input = Vector2.zero;
        }
    }
    public void OnPointerUp(PointerEventData eventData) // 떼면 제자리로
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
