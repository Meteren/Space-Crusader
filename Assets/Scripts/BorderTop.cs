using UnityEngine;

public class BorderTop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Bullet>(out Bullet bullet))
        {
            if (bullet.gameObject.activeSelf)
            {
                if(bullet.gameObject.activeSelf)
                    bullet.Release();
            }
        }
    }
}
