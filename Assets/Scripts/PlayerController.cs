
using UnityEngine;
using System.Collections.Generic;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using System.Collections;
using JetBrains.Annotations;

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
    [SerializeField] private GameObject shipExplosionEffect;
    [SerializeField,HideInInspector] private float particleDuration;
    [SerializeField] private float particleStart;

    private BoxCollider2D boundary;
    
    private Vector2 touchBegin;
    private Vector2 capturedDeltaTouch;

    private Camera cam;

    Quaternion rotateTo;

    [HideInInspector] public float movePosXMin;
    [HideInInspector] public float movePosXMax;

    public Vector2 boundarySize => boundary.bounds.size;


    List<Heart> hearts = new();

    public int health = 3;

    bool cantBeDamaged;

    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Transform parentHeart = GameObject.Find("Hearts").transform;

        for(int i = 0; i < parentHeart.childCount; i++)
            hearts.Add(parentHeart.GetChild(i).GetComponent<Heart>());


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
            if (!float.IsInfinity(TouchManager.instance.touch.screenPosition.x) || !float.IsInfinity(TouchManager.instance.touch.screenPosition.y))
            {
                if (TouchManager.instance.touch.phase == TouchPhase.Began)
                {
                    touchBegin = Camera.main.ScreenToWorldPoint(TouchManager.instance.touch.screenPosition);
                }
                if (TouchManager.instance.touch.phase == TouchPhase.Moved || TouchManager.instance.touch.phase == TouchPhase.Stationary)
                {
                    Vector2 touchMovePos = Camera.main.ScreenToWorldPoint(TouchManager.instance.touch.screenPosition);
                    deltaTouch = touchMovePos - touchBegin;
                    touchBegin = touchMovePos;
                    return true;
                }
            }
           
        }
        deltaTouch = Vector2.zero;
        return false;
    }

    private bool ClampPosition(ref Vector2 positionToClamp)
    {
        movePosXMin = cam.ScreenToWorldPoint(new Vector2(0, 0)).x + boundarySize.x / 2 + offsetX;
        movePosXMax = cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - boundarySize.x / 2 - offsetX;

        bool isClamped = positionToClamp.x < movePosXMin || positionToClamp.x >= movePosXMax; 

        positionToClamp = new Vector2(Mathf.Clamp(positionToClamp.x, movePosXMin, movePosXMax), positionToClamp.y);

        return isClamped;
    }

    public void Init(Vector2 position)
    {
        transform.position = position;
        transform.localScale /= CameraScaler.scaleFactorX;
    }

    public void OnDamage()
    {
        health--;

        hearts[(hearts.Count - 1) - health].ReleaseHeart();

        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(shipExplosionEffect,transform.position,Quaternion.identity);
        }

        StartCoroutine(WaitForNextDamageRoutine());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamageable<Bullet>>() != null && !cantBeDamaged)
        {
            Debug.Log("Player got damage");
            cantBeDamaged = true;
            OnDamage();
        }
            
    }

    private IEnumerator WaitForNextDamageRoutine()
    {
        for(int i = 0; i < 10; i++)
        {
   
            Color shipColor = sr.color;

            var main = rocketParticle.main;

            Color rocketParticleColor = main.startColor.color;

            yield return new WaitForSeconds(0.1f);

            shipColor.a = 0;
            rocketParticleColor.a = 0;

            sr.color = shipColor;
            main.startColor = rocketParticleColor;


            yield return new WaitForSeconds(0.1f);

            shipColor.a = 1;
            rocketParticleColor.a = 1;

            sr.color = shipColor;
            main.startColor = rocketParticleColor;

        }
        cantBeDamaged = false;
        
    }

}
