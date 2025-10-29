using UnityEngine;

public class ScatteredBullet : Bullet
{
    [SerializeField] private float posOffset;

    Vector2 normalizedDirection;
    float baseAngle => (updatedData.angleBetweenMultipleShots * updatedData.shotsToReflectCount + 90f);
    public override void ApplyModeSpecification()
    {
        SetPosition();
    }

    public override void Release()
    {
        base.Release();
    }

    public override void SetPosition()
    {
        InitScatterStates(0);             
    }

    public void InitScatterStates(int bulletIndex)
    {
       
        float currentAngle = baseAngle - bulletIndex * updatedData.angleBetweenMultipleShots;

        float rotationVal = currentAngle - 90f;
 
        currentAngle *= Mathf.Deg2Rad;

        Vector3 startPos = playerReference.transform.position + new Vector3(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0) * posOffset;
        Vector3 reflectedStartPos = playerReference.transform.position + new Vector3(-Mathf.Cos(currentAngle), Mathf.Sin(currentAngle), 0) * posOffset;

        transform.position = startPos;

        transform.rotation = Quaternion.Euler(0, 0, rotationVal + 90f);

        normalizedDirection = (transform.position - playerReference.transform.position).normalized;

        SetSpeed();

        ScatteredBullet reflectedScatteredBullet = (ScatteredBullet)bulletPoolBelonged.Get();

        reflectedScatteredBullet.transform.position = reflectedStartPos;

        reflectedScatteredBullet.transform.rotation = Quaternion.Euler(0, 0, -rotationVal + 90f);

        reflectedScatteredBullet.Init(dataReference, bulletPoolBelonged, playerReference, reflectedStartPos);

        reflectedScatteredBullet.normalizedDirection = new Vector2(-normalizedDirection.x, normalizedDirection.y);

        reflectedScatteredBullet.SetSpeed();

        if (bulletIndex == 0)
            SoundManager.instance.PlaySFX(baseData.shootingSFX, 1f);

        if(bulletIndex < updatedData.shotsToReflectCount - 1)
        {
            ScatteredBullet nextScatteredBullet = (ScatteredBullet)bulletPoolBelonged.Get();

            nextScatteredBullet.Init(dataReference, bulletPoolBelonged, playerReference, playerReference.transform.position);
            nextScatteredBullet.InitScatterStates(++bulletIndex);

        }
      
    }

    public override void SetSpeed()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(normalizedDirection.x * updatedData.speed.x, normalizedDirection.y * updatedData.speed.y);

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
