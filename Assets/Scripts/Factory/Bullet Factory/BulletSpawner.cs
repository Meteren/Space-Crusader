using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class BulletSpawner : MonoBehaviour
{

    [Header("Generation Pool For Bullets")]
    [SerializeField] private List<BulletData> bulletDataReferences; 
    [HideInInspector]public List<BulletData> bulletDataInstances = new();
    BulletFactory bulletFactory;

    private PlayerController playerController;

    EffectGridController effectGridController;

    private void Awake()
    {
        effectGridController = FindFirstObjectByType<EffectGridController>();
        playerController = GetComponent<PlayerController>();
        bulletFactory = new BulletFactory(playerController);
        
        BulletData instantiatedData = Instantiate(bulletDataReferences[0],transform);
        bulletDataInstances.Add(instantiatedData);
        
        
    }

    private void Update()
    {
        if (TouchManager.instance.activeTouchesCount > 0 && !effectGridController.gameObject.activeSelf || Input.GetMouseButtonDown(0) )
        {
            foreach (var data in bulletDataInstances)
            {
                if (data.bulletReadyToUse)
                {
                    data.countDown -= Time.deltaTime;
                    if (data.countDown <= 0)
                    {
                        bulletFactory.Create(data, playerController.transform.position);
                        data.countDown = data.generationTime;
                    }

                }
             }
 
        }
    }

    public void AddEffect(IEffect<Bullet> effect)
    {
        
        BulletData dataToBeEffected = GetDataToBeEffected(effect);

        //Check if its null later, leave it for now

        dataToBeEffected.effects.Add(effect);
        //Debug.Log($"{dataToBeEffected.prefab.GetType().Name} effects count: {dataToBeEffected.effects.Count}");
  
    }

    public bool TryGetEffect(Type effectType,Type targetType, out IEffect<Bullet> effect)
    {

        BulletData dataToBeEffected = GetDataToBeEffected(targetType);

        if(dataToBeEffected != null)
        {
            if (dataToBeEffected.effects.Count > 0)
            {
                effect = dataToBeEffected.effects.Select(x => x).Where(x => (x is EffectResolver manager ? manager.EffectType
                             : x.GetType()) == effectType).FirstOrDefault();

                if (effect != null)
                    return true;
            }

        }

        effect = default;
        return false;

    }
    
    public void RemoveEffect(IEffect<Bullet> effect)
    {
        BulletData dataToBeEffected = GetDataToBeEffected(effect);
        if(dataToBeEffected != null)
            dataToBeEffected.effects.Remove(effect);
    }


    public void AddBulletToSpawner(Type bulletType)
    {
        
        for (int i = 0; i < bulletDataReferences.Count; i++)
        {
            if (bulletDataReferences[i].bulletType == bulletType)
            {
                BulletData newBulletData = Instantiate(bulletDataReferences[i], transform);
                bulletDataInstances.Add(newBulletData);
                break;
            }
                
        }

    }

    public void RemoveBulletFromSpawner(Type bulletType)
    {
        for (int i = 0; i < bulletDataReferences.Count; i++)
        {
            if (bulletDataInstances[i].prefab.GetType() == bulletType)
            {
                BulletData foundBulletData = bulletDataInstances[i];
                bulletDataInstances.Remove(foundBulletData);
                break;
            }

        }
    }

    public BulletData GetDataToBeEffected(IEffect<Bullet> effect)
    {
        EffectResolver resolver = effect as EffectResolver;
        if(resolver != null)
        {
            BulletData data = bulletDataInstances.FirstOrDefault(x => x.bulletType == resolver.TargetType);
            return data;
        }else
            return null;

    }

    public BulletData GetDataToBeEffected(Type targetType)
    {
        BulletData dataFound = bulletDataInstances.FirstOrDefault(x => x.bulletType == targetType);

        return dataFound;
    }

}

