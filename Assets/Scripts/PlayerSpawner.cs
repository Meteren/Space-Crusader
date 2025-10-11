using System;
using UnityEngine;

[Serializable]
public class PlayerSpawner
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    public void Spawn() 
    {
        PlayerController instantiatedPlayer = GameObject.Instantiate(playerPrefab).GetComponent<PlayerController>();

        instantiatedPlayer.Init(spawnPoint.position);

    }

}
