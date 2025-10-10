using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerController : MonoBehaviour
{

    [Header("Player Speed")]
    [SerializeField] private float speed;

    [Header("Offset For Boundary")]
    [SerializeField] private float offsetX;

    private BoxCollider2D boundary;
    

    private Vector2 touchBegin;
    private Vector2 capturedDeltaTouch;

    private Camera cam;

    Vector2 boundarySize => boundary.bounds.size;

    private void Start()
    {
        boundary = GetComponent<BoxCollider2D>();
        cam = Camera.main;         
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
#if UNITY_EDITOR
        TouchSimulation.Enable();
#endif
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
#if UNITY_EDITOR
        TouchSimulation.Disable();
#endif
    }
    private void Update()
    {

        if (TryGetDeltaTouch(out Vector2 deltaTouch))
            capturedDeltaTouch = deltaTouch; //incrementation for frame independency
        else
            capturedDeltaTouch = Vector2.zero;

        transform.position = Vector2.MoveTowards(transform.position, transform.position +
        new Vector3(capturedDeltaTouch.x, 0, 0), speed * Time.deltaTime);

        ClampPlayerPosition();
    }

    private void FixedUpdate()
    {
       /* if (!inCollisionWithBoundaries)
            rb.linearVelocity = new Vector2(capturedDeltaTouch.x * speed, rb.linearVelocityY);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);

        capturedDeltaTouch = Vector2.zero;*/


    }

    private bool TryGetDeltaTouch(out Vector2 deltaTouch)
    {
        var activeTouches = Touch.activeTouches;
        if (activeTouches.Count > 0)
        {
            Debug.Log("Touch detected.");
            var touch = activeTouches[0];
            if (touch.phase == TouchPhase.Began)
            {     
                touchBegin = cam.ScreenToWorldPoint(touch.screenPosition);
                Debug.Log($"Touch begin point:{touchBegin}");
            }
            if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector2 touchMovePos = cam.ScreenToWorldPoint(touch.screenPosition);
                deltaTouch = touchMovePos - touchBegin;
                touchBegin = touchMovePos;
                Debug.Log($"Touch delta:{deltaTouch}");
                return true;
            }

        }
        deltaTouch = Vector2.zero;
        return false;
    }

    private void ClampPlayerPosition()
    {
        float minX = cam.ScreenToWorldPoint(new Vector2(0, 0)).x + boundarySize.x / 2 + offsetX;
        float maxX = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - boundarySize.x / 2 - offsetX;

        transform.position = new Vector2(Mathf.Clamp(transform.position.x,minX, maxX), transform.position.y);

    }

}
