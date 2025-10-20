using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    List<IDamageable<Bullet>> targetEnemies = new List<IDamageable<Bullet>>();

    private void Update()
    {
        foreach (var e in targetEnemies.ToList())
        {
            if (e is Asteroid asteroid)
                if (asteroid.isDestroyed)
                    targetEnemies.Remove(e);
        }
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Asteroid>(out Asteroid asteroid))
            if(!targetEnemies.Contains(asteroid))
                targetEnemies.Add(asteroid);
    }

    public bool TryGetFirstTargetLocated(out Transform firstTarget)
    {
        if(targetEnemies.Count > 0)
        {
            IDamageable<Bullet> damageableTarget = targetEnemies.FirstOrDefault(x =>
            {
                if (x is Asteroid asteroid)
                    if (!asteroid.isDestroyed)
                    {
                        EffectResolver effectResolver = asteroid.ActiveEffects
                                    .OfType<EffectResolver>()
                                    .FirstOrDefault(x => x.SourceType == typeof(ExplosiveBullet));

                        if (effectResolver != null)
                            return false;
                        else
                            return true;
                    }

                return false;

            });

            if(damageableTarget != null)
            {
                Asteroid asteroid = (Asteroid)damageableTarget;
                firstTarget = asteroid.transform;
                return true;
            }

        } 
        firstTarget = default;
        return false;
    }

    private void OnDisable() =>
        targetEnemies.Clear();

}
