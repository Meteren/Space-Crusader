using System;
using UnityEngine;

[Serializable]
public class PlayerSpawner
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    public void Spawn() 
    {
        if (spawnPoint == null)
            spawnPoint = GameObject.Find("PlayerSpawnPos").transform;
        PlayerController instantiatedPlayer = GameObject.Instantiate(playerPrefab).GetComponent<PlayerController>();
        instantiatedPlayer.transform.localScale *= CameraScaler.scaleFactorX;
        
        instantiatedPlayer.Init(spawnPoint.position);

    }

}
