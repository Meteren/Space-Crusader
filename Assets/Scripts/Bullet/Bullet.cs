
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    protected BulletData data;
    protected IObjectPool<Bullet> bulletPoolBelonged;

    private Rigidbody2D rb;
    public IObjectPool<Bullet> BulletPoolBelonged { get => bulletPoolBelonged; }
    public BulletData Data { get => data; }

    private void Start()
    {
        transform.localScale /= CameraScaler.scaleFactor;
    }

    public void Init(BulletData data, IObjectPool<Bullet> bulletPoolBelonged,PlayerController pc,Vector2 position)
    {

        this.data = data;
        this.bulletPoolBelonged = bulletPoolBelonged;
        transform.position = position + new Vector2(0, pc.boundarySize.y / 2);
        SetSpeed(pc);
    }

    private void SetSpeed(PlayerController pc)
    {
        if(!rb)
            rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = data.speed;
    }

}
