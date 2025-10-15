using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ScriptableObjects/BulletData")]
public class BulletData : ScriptableObject
{
    //can be extended
    [Header("Bullet Prefab")]
    public Bullet prefab;

    public Type bulletType => prefab.GetType();

    //----
    [Header("Bullet Attributes")]
    public bool bulletReadyToUse;
    public float generationTime;
    [SerializeField] private Vector2 speed;
    [SerializeField] private int pierceCount;

    [SerializeField] private bool canExplode;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBurstShots;

    [SerializeField] private bool multipleShot;

    public int bulletDataIndex;
    //---

    [HideInInspector] public readonly List<IEffect<Bullet>> effects = new();

    [HideInInspector] public float countDown;


    //for data extraction--
    public struct DataFields
    {
        public float generationTime;
        public Vector2 speed;
        public bool bulletReadyToUse;
        public int pierceCount;
        public bool canExplode;
        public float timeBetweenBurstShots;
        public bool multipleShot;
        public bool burstMode;  
        public int burstCount;
        
    }
    //--
    public virtual DataFields CopyClassInstance()
    {
        DataFields dataFields = new DataFields();
        dataFields.generationTime = generationTime;
        dataFields.speed = speed;
        dataFields.bulletReadyToUse = bulletReadyToUse;
        dataFields.pierceCount = pierceCount;
        dataFields.canExplode = canExplode;
        dataFields.timeBetweenBurstShots = timeBetweenBurstShots;
        dataFields.multipleShot = multipleShot;
        dataFields.burstCount = burstCount;

        return dataFields;
    }


}
