using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class BulletSpawner : MonoBehaviour
{
    public readonly List<IEffect<Bullet>> bulletEffects = new();
  
    [SerializeField] private List<BulletData> bulletData; //later add a selection for specified bullet according to effect list in player
                                                          //can be referenced to player from here for the selection logic
    BulletFactory bulletFactory;

    private PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        bulletFactory = new BulletFactory(playerController);
    }

    private void Update()
    {
        if (TouchManager.instance.activeTouchesCount > 0 && !UIManager.instance.effectSelectionScreen.activeSelf || Input.GetMouseButtonDown(0) )
        {
            {
                foreach (var data in bulletData)
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
    }

    public void AddEffect(IEffect<Bullet> effect)
    {
        bulletEffects.Add(effect);
        bulletFactory.SetEffects(bulletEffects);
        Debug.Log($"Bullet effects count:{bulletEffects.Count}");
    }

    public bool TryGetEffect(Type type, out IEffect<Bullet> effect)
    {
        if(bulletEffects.Count > 0)
        {
            effect = bulletEffects.Select(x => x).Where(x => (x is EffectResolver manager ? manager.Type : x.GetType()) == type).FirstOrDefault();
            
            if(effect != null)
                return true;
        }
            
        effect = default;
        return false;

    }
    
    public void RemoveEffect(IEffect<Bullet> effect)
    {
        bulletEffects.Remove(effect);
        bulletFactory.SetEffects(bulletEffects);
    }


}

