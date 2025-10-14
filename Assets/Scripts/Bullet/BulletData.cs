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
    public float generationTime;
    public Vector2 speed;
    public bool bulletReadyToUse;
    public int pierceCount;
    //---
    [HideInInspector]public readonly List<IEffect<Bullet>> effects = new();

    [HideInInspector] public float countDown;

    public int bulletDataIndex;

    public struct DataFields
    {
        public float generationTime;
        public Vector2 speed;
        public bool bulletReadyToUse;
        public int pierceCount;
    }

    public DataFields CopyClassInstance()
    {
        DataFields dataFields = new DataFields();
        dataFields.generationTime = generationTime;
        dataFields.speed = speed;
        dataFields.bulletReadyToUse = bulletReadyToUse;
        dataFields.pierceCount = pierceCount;

        return dataFields;
    }


}
