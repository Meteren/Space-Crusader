using UnityEngine;

public class SpawnPos : MonoBehaviour
{
    [SerializeField] private float offsetY;
    void Start()
    {
        Vector2 camPos = Camera.main.transform.position;
        transform.position = new Vector2(camPos.x, camPos.y + offsetY / CameraScaler.scaleFactor);
    }


}
