using System.Collections;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    Interval timer;
    int choosedSide;
    float angle = 0;

    float interpolationVal = 0;

    float constantClampedX;
    float clampedX;

    float capturedBulletXPos;

    [Header("Explosive Bullet Attributes")]
    [SerializeField] float interpolationSpeed;
    [SerializeField] float startAngle;
    [SerializeField] float finishAngle;
    [SerializeField] float orbitalMoveDuration;
    enum Side
    {
        Left, Right
    }
    private void Update()
    {
        SetSpeed();
    }
    public override void ApplyModeSpecification()
    {
        timer = new Interval(updatedData.timeBetweenBurstShots, updatedData.timeBetweenBurstShots * updatedData.burstCount,"Explosive Burst Timer");
        timer.onInterval += BurstAction;
        timer.StartTimer();

        SetIndividualValues();
    }

    public override void SetSpeed()
    {
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
            explosiveBullet.Init(dataReference,bulletPoolBelonged,playerReference,playerReference.transform.position,storedEffects);;
        explosiveBullet.SetIndividualValues();
    }

    private float GetDistanceBetween(float pointA, float pointB)
    {
        return Mathf.Abs(pointB - pointA); 
    }

    public float GetConstantClampedX(ExplosiveBullet explosiveBullet)
    {
        return explosiveBullet.choosedSide > 0 ? GetDistanceBetween(explosiveBullet.capturedBulletXPos, explosiveBullet.playerReference.movePosXMax)
           : GetDistanceBetween(capturedBulletXPos, playerReference.movePosXMin);
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
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        //collision logic for explosin will be implemented in here
    }
    

}

