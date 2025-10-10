
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerController : MonoBehaviour
{

    [Header("Player Speed")]
    [SerializeField] private float moveSpeed;

    [Header("Rotation Segment")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationExtension;

    [Header("Offset For Boundary")]
    [SerializeField] private float offsetX;

    private BoxCollider2D boundary;
    

    private Vector2 touchBegin;
    private Vector2 capturedDeltaTouch;

    private Camera cam;

    Quaternion rotateTo;

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
            capturedDeltaTouch = deltaTouch;
        else
            capturedDeltaTouch = Vector2.zero;

        transform.position = Vector2.MoveTowards(transform.position, transform.position +
        new Vector3(capturedDeltaTouch.x, 0, 0), moveSpeed * Time.deltaTime);

        float angle = Mathf.Atan2(1, capturedDeltaTouch.x * rotationExtension) * Mathf.Rad2Deg - 90.0f;

        rotateTo = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * rotationSpeed);

        ClampPlayerPosition();
    }

    private void FixedUpdate()
    {
        /* if (!inCollisionWithBoundaries)
             rb.linearVelocity = new Vector2(capturedDeltaTouch.x * speed, rb.linearVelocityY);
         else
             rb.linearVelocity = new Vector2(0, rb.linearVelocityY);

         capturedDeltaTouch = Vector2.zero;*/

        //capturedDeltaTouch = Vector2.zero;

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
