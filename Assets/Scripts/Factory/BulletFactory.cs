using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletFactory : IFactory<Bullet,BulletData>
{
    Transform parent;
    private Dictionary<BulletData, IObjectPool<Bullet>> bulletPool = new();
    PlayerController playerController;

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

        List<IEffect<Bullet>> effects = data.effects;

        Bullet bullet = pool.Get();
        if(effects != null)
            Debug.Log($"Effects: {effects.Count}");
        bullet.Init(data,pool,playerController,position, effects);
        return bullet;
    }


}


