using UnityEngine;
using System.Collections.Generic;

public class BulletSpawner : MonoBehaviour
{
                                                                         
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
        if(TouchManager.instance.activeTouchesCount > 0)
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


