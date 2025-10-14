using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletFactory : IFactory<Bullet,BulletData>, IFactoryEffectResolver<Bullet>
{
    Transform parent;
    private Dictionary<BulletData, IObjectPool<Bullet>> bulletPool = new();
    PlayerController playerController;

    public List<IEffect<Bullet>> Effects { get; set; }

    public BulletFactory(PlayerController playerController, Transform parent = null)
    {
        this.parent = parent;
        this.playerController = playerController;
    }

    public Bullet Create(BulletData data, Vector2 position)
    {
        var pool = bulletPool.TryGetValue(data,out IObjectPool<Bullet> oPool) ? oPool
            : new ObjectPool<Bullet>(
                () => Object.Instantiate(data.prefab,parent).GetComponent<Bullet>(),
                bullet => bullet.gameObject.SetActive(true),
                bullet => bullet.gameObject.SetActive(false),
                bullet => GameObject.Destroy(bullet.gameObject),
                true,
                10,
                50
                ); 

        bulletPool[data] = pool;

        Bullet bullet = pool.Get();
        if(Effects != null)
            Debug.Log($"Effects: {Effects.Count}");
        bullet.Init(data,pool,playerController,position, Effects);
        return bullet;
    }

    public void SetEffects(List<IEffect<Bullet>> effects)
    {
        
        Effects = effects;
        Debug.Log($"Setted effects:{Effects.Count}");
    }
}


