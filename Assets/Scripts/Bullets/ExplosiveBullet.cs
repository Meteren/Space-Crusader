
using System.Collections;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    Interval timer;
    int choosedSide;
    float angle = 0;
    float angleX = 0;
    float angleY = 0;   

    float interpolationVal = 0;

    float interPolationValX = 0;
    float interPolationValY = 0;

    float constantClampedX;
    float clampedX;

    float capturedBulletXPos;

    bool inImpact;

    [Header("Explosive Bullet Attributes")]
    [SerializeField] float interpolationSpeed;
    [SerializeField] float startAngle;
    [SerializeField] float finishAngle;
    [SerializeField] float orbitalMoveDuration;

    [Header("Impact Values")]
    [SerializeField] float impactRadius;
    [SerializeField] private float impactTime;
    [SerializeField] private int impactInterval;
    [SerializeField] private LayerMask layersToBeEffected;

    public float ImpactRadius { get => impactRadius;}

    [SerializeField] private TargetLocator targetLocator;

    //--- target locating system

    Transform capturedTarget;
    float targetLockStartAngle;
    float targetLockEndAngle;
    Vector2 targetDirection;
    float constantDistanceX;
    float distanceX;
    float constantDistanceY;
    float distanceY;

    //--
    private void Awake()
    {
        targetLocator = GetComponentInChildren<TargetLocator>();
    }

    enum Side
    {
        Left, Right
    }
    private void Update()
    {
        if (targetLocator.TryGetFirstTargetLocated(out Transform firstTargetLocated))
            MoveToTarget(firstTargetLocated);        
        else
            SetSpeed();

    }

    public override void ApplyModeSpecification()
    {
        timer = new Interval(updatedData.timeBetweenBurstShots, updatedData.timeBetweenBurstShots * updatedData.burstCount,"Explosive Burst Timer");
        timer.onInterval += BurstAction;
        timer.StartTimer();

        SetIndividualValues();
    }

    private void MoveToTarget(Transform target)
    {
        if (inImpact)
            return;

        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        if(target != capturedTarget)
        {

            capturedTarget = target;

            Debug.Log($"Target setted to {capturedTarget}");
            targetLockStartAngle = Mathf.Atan2(rb.linearVelocityY, rb.linearVelocityX) * Mathf.Rad2Deg;
            targetDirection = target.position - transform.position;
            Vector2 normalizedDir = targetDirection.normalized;
            targetLockEndAngle = Mathf.Atan2(normalizedDir.y, normalizedDir.x) * Mathf.Rad2Deg;

            if (targetLockStartAngle < 0)
                targetLockEndAngle += 360;

            if (targetLockEndAngle < 0)
                targetLockEndAngle += 360;

            constantDistanceX = Mathf.Abs(targetDirection.x);
            distanceX = constantDistanceX;
            constantDistanceY = Mathf.Abs(targetDirection.y);
            distanceY = constantDistanceY;
        }
        targetDirection = target.position - transform.position;
        Vector2 nDirection = targetDirection.normalized;

        Debug.Log($"Normalized Dir: {nDirection}");
      
        /*distanceX -= interpolationSpeed * (Time.deltaTime / orbitalMoveDuration);
        distanceY -= interpolationSpeed * (Time.deltaTime / orbitalMoveDuration);
        interPolationValX = Mathf.InverseLerp(constantDistanceX, 0, distanceX);
        interPolationValY = Mathf.InverseLerp(constantDistanceY, 0, distanceY);

        float fixedInterpolationX = Mathf.SmoothStep(0, 1, interPolationValX);
        float fixedInterpolationY = Mathf.SmoothStep(0, 1, interPolationValY);
        Debug.Log($"Interpolation vals: x:{fixedInterpolationX} - y: {fixedInterpolationY}");

        angleX = Mathf.Lerp(targetLockStartAngle, targetLockEndAngle, interPolationValX);

        float radianX = angleX * Mathf.Deg2Rad;
        float radianY = angleY * Mathf.Deg2Rad;

        float speedX = Mathf.Lerp(rb.linearVelocityX, targetDirection.x, fixedInterpolationX);
        float speedY = Mathf.Lerp(rb.linearVelocityY, targetDirection.y, fixedInterpolationY);   */

        rb.linearVelocity = nDirection * updatedData.speed.magnitude * 0.5f;
        Debug.Log($"Explosive bullet speed {rb.linearVelocity}");
    }
    public override void SetSpeed()
    {
        Debug.Log("Speeding");

        if (inImpact)
            return;

        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        clampedX -= interpolationSpeed * (Time.deltaTime / orbitalMoveDuration);
        interpolationVal = Mathf.InverseLerp(constantClampedX, 0, clampedX);

        angle = Mathf.Lerp(startAngle, finishAngle, interpolationVal);

        float radian = angle * Mathf.Deg2Rad;

        Vector2 speedVector = new Vector2(Mathf.Cos(radian) * choosedSide * updatedData.speed.x, Mathf.Sin(radian) * updatedData.speed.y);

        rb.linearVelocity = speedVector;
    }
    public override void SetPosition()
    {
        choosedSide = ChooseSideToMove();
        transform.position = 
            new Vector3(playerReference.transform.position.x + 
            (choosedSide * playerReference.boundarySize.x / 2), playerReference.transform.position.y,0);
    }

    private int ChooseSideToMove()
    {
        int sideAsNumber = Random.Range(0, 2);

        return sideAsNumber == (int)Side.Right ? (int)Side.Right : (int)Side.Left - 1;
    }

    public void BurstAction()
    {
        Debug.Log("Burst action");
        ExplosiveBullet explosiveBullet = bulletPoolBelonged.Get() as ExplosiveBullet;
        if(explosiveBullet != null)
            explosiveBullet.Init(dataReference,bulletPoolBelonged,playerReference,playerReference.transform.position);;
        explosiveBullet.SetIndividualValues();
    }

    private float GetDistanceBetween(float pointA, float pointB)
    {
        return Mathf.Abs(pointB - pointA); 
    }

    public void SetIndividualValues()
    {
        SetPosition();
        capturedBulletXPos = transform.position.x;
        constantClampedX = choosedSide > 0 ? GetDistanceBetween(capturedBulletXPos, playerReference.movePosXMax)
           : GetDistanceBetween(capturedBulletXPos, playerReference.movePosXMin);
        clampedX = constantClampedX;
    }

    public override void Release()
    {
        base.Release();
        interpolationVal = 0;
        rb.linearVelocity = Vector2.zero;
        inImpact = false;
        capturedTarget = null;
    }
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision Detected for explosion bullet");
        //collision logic for explosion will be implemented in here
        if (collision.GetComponent<Asteroid>() != null && !collision.GetComponent<Bullet>() && !inImpact)
        {
            inImpact = true;
            Collider2D[] asteroidsToBeEffected = Physics2D.OverlapCircleAll(transform.position, impactRadius,layersToBeEffected);


            for (int i = 0; i < asteroidsToBeEffected.Length; i++)
            {
                Asteroid asteroid = asteroidsToBeEffected[i].transform.GetComponent<Asteroid>();
                asteroid.AddEffect(new DamageOverTime<Bullet>(this,impactTime,impactInterval));
                
            }
            rb.linearVelocity = Vector2.zero;
            CountDown countDownToDestroy = new CountDown(impactTime, GetType().Name);
            countDownToDestroy.onEnd += Release;
            countDownToDestroy.StartTimer();

            //play a particle effect
        }
        
    }
    
}

