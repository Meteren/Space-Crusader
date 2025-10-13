using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BulletData")]
public class BulletData : ScriptableObject
{
    //can be extended
    [Header("Bullet Prefab")]
    public GameObject prefab;

    //----
    [Header("Bullet Attributes")]
    public float generationTime;
    public Vector2 speed;
    public bool bulletReadyToUse;
    public int pierceCount;
    //---

    [HideInInspector] public float countDown;

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
