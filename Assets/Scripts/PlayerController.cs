
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

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

    [Header("Particle")]
    [SerializeField] private ParticleSystem rocketParticle;
    [SerializeField,HideInInspector] private float particleDuration;
    [SerializeField] private float particleStart;

    private BoxCollider2D boundary;
    
    private Vector2 touchBegin;
    private Vector2 capturedDeltaTouch;

    private Camera cam;

    Quaternion rotateTo;

    public Vector2 boundarySize => boundary.bounds.size;

    private void Start()
    {
        boundary = GetComponent<BoxCollider2D>();
        cam = Camera.main;
        rocketParticle.Play();
        particleDuration = rocketParticle.main.duration;
    }
    private void Update()
    {
        //handle particles to stop generating every frame for performance
        particleDuration -= Time.deltaTime;
        if(particleDuration <= 0)
        {
            rocketParticle.Stop();
            if(particleDuration <= particleStart)
            {
                rocketParticle.Play();
                particleDuration = rocketParticle.main.duration;
            }
        }

        if (TryGetDeltaTouch(out Vector2 deltaTouch))
            capturedDeltaTouch = deltaTouch;
        else
            capturedDeltaTouch = Vector2.zero;

        Vector2 currentPosition = transform.position;

        currentPosition = Vector2.MoveTowards(currentPosition, transform.position +
        new Vector3(capturedDeltaTouch.x, 0, 0), moveSpeed * Time.deltaTime);

        bool isClamped = ClampPosition(ref currentPosition);

        transform.position = currentPosition;

        float angle = Mathf.Atan2(1, capturedDeltaTouch.x * rotationExtension) * Mathf.Rad2Deg - 90.0f;

        if (!isClamped)
        {           
            rotateTo = Quaternion.Euler(0, 0, angle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, Time.deltaTime * rotationSpeed);
        }
        else
        {
            rotateTo = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation,rotateTo, Time.deltaTime * rotationSpeed);
        }


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
        if (TouchManager.instance.activeTouchesCount > 0)
        {
            if (TouchManager.instance.touch.phase == TouchPhase.Began)
            {     
                touchBegin = cam.ScreenToWorldPoint(TouchManager.instance.touch.screenPosition);
                Debug.Log($"Touch begin point:{touchBegin}");
            }
            if(TouchManager.instance.touch.phase == TouchPhase.Moved || TouchManager.instance.touch.phase == TouchPhase.Stationary)
            {
                Vector2 touchMovePos = cam.ScreenToWorldPoint(TouchManager.instance.touch.screenPosition);
                deltaTouch = touchMovePos - touchBegin;
                touchBegin = touchMovePos;
                Debug.Log($"Touch delta:{deltaTouch}");
                return true;
            }

        }
        deltaTouch = Vector2.zero;
        return false;
    }

    private bool ClampPosition(ref Vector2 positionToClamp)
    {
        float minX = cam.ScreenToWorldPoint(new Vector2(0, 0)).x + boundarySize.x / 2 + offsetX;
        float maxX = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - boundarySize.x / 2 - offsetX;

        bool isClamped = positionToClamp.x < minX || positionToClamp.x >= maxX; 

        positionToClamp = new Vector2(Mathf.Clamp(positionToClamp.x,minX, maxX), positionToClamp.y);

        return isClamped;
    }

    public void Init(Vector2 position)
    {
        transform.position = position;
    }
}
