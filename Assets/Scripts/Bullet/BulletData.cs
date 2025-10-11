using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BulletData")]
public class BulletData : ScriptableObject
{
    //can be extended
    [Header("Bullet Prefab")]
    public GameObject prefab;

    [Header("Bullet Attributes")]
    public float generationTime;
    public Vector2 speed;
    public bool bulletReadyToUse;

    public float countDown;

}
