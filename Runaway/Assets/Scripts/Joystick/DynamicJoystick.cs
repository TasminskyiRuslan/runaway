/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}*/
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    private Vector2 originalPosition;
    private RectTransform canvasRectTransform;

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        originalPosition = background.anchoredPosition;
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        background.anchoredPosition = ClampToScreen(localPoint);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        background.anchoredPosition = originalPosition;
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            Vector2 newPosition = background.anchoredPosition + difference;
            background.anchoredPosition = ClampToScreen(newPosition);
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }

    private Vector2 ClampToScreen(Vector2 position)
    {
        Vector2 clampedPosition = position;
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // Clamp the position within the canvas boundaries
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0, canvasSize.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0, canvasSize.y);

        return clampedPosition;
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    private Vector2 originalPosition;
    private RectTransform canvasRectTransform;
    private RectTransform backgroundRectTransform;

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        originalPosition = background.anchoredPosition;
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        backgroundRectTransform = background.GetComponent<RectTransform>();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, eventData.position, eventData.pressEventCamera, out localPoint);
        background.anchoredPosition = ClampToScreen(localPoint);
       // background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
       // background.gameObject.SetActive(false);
        background.anchoredPosition = originalPosition;
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            Vector2 newPosition = background.anchoredPosition + difference;
            background.anchoredPosition = ClampToScreen(newPosition);
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }

    private Vector2 ClampToScreen(Vector2 position)
    {
        Vector2 clampedPosition = position;
        Vector2 canvasSize = canvasRectTransform.sizeDelta;
        Vector2 halfBackgroundSize = backgroundRectTransform.sizeDelta / 2;

        // Clamp the position within the canvas boundaries considering the size of the joystick background
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, halfBackgroundSize.x, canvasSize.x - halfBackgroundSize.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, halfBackgroundSize.y, canvasSize.y - halfBackgroundSize.y);

        return clampedPosition;
    }
}
