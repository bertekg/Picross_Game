using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PressAndHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;

    [SerializeField]
    private float requiredHoldTime;

    public UnityEvent onLongClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        pointerDownTimer = requiredHoldTime;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }
    private void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if (pointerDownTimer >= requiredHoldTime)
            {
                if (onLongClick != null)
                {
                    onLongClick.Invoke();
                }
                pointerDownTimer -= requiredHoldTime;
            }
        }
    }
    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
    }
}
