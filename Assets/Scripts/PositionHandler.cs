using System;
using UnityEngine;

public class PositionHandler : MonoBehaviour
{
    [SerializeField] private float offsetY;
    void Start()
    {
        Vector2 screenPoint = new Vector2(Screen.width / 2, (Screen.height / 2 + offsetY) * CameraScaler.scaleFactorY);

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

        float zVal = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);

        transform.position = new Vector3(worldPoint.x, worldPoint.y, zVal);
    }


}
