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
    public float defaultGenTime;
    [SerializeField] private Vector2 speed;
    [SerializeField] private int pierceCount;
    [SerializeField] private int damageAmount;

    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBurstShots;

    [SerializeField] private int shotsToReflectCount;
    [SerializeField] private float angleBetweenMultipleShots;

    [SerializeField] private float fireRateMuliplyVal;

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
        public float timeBetweenBurstShots;  
        public int burstCount;
        public float defaultGenTime;
        public int shotsToReflectCount;
        public float angleBetweenMultipleShots;
        public float fireRateMuliplyVal;
        public int damageAmount;
        
    }
    //--
    public virtual DataFields CopyClassInstance()
    {
        DataFields dataFields = new DataFields();
        dataFields.generationTime = generationTime;
        dataFields.speed = speed;
        dataFields.bulletReadyToUse = bulletReadyToUse;
        dataFields.pierceCount = pierceCount;
        dataFields.timeBetweenBurstShots = timeBetweenBurstShots;
        dataFields.burstCount = burstCount;
        dataFields.defaultGenTime = defaultGenTime;
        dataFields.angleBetweenMultipleShots = angleBetweenMultipleShots;
        dataFields.shotsToReflectCount = shotsToReflectCount;
        dataFields.fireRateMuliplyVal = fireRateMuliplyVal;
        dataFields.damageAmount = damageAmount;

        return dataFields;
    }


}
