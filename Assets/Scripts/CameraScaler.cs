using UnityEngine;
using UnityEngine.UI;

public class CameraScaler : MonoBehaviour
{
    private GameObject canvas;
    [SerializeField] private float referenceWidth;

    Camera cam;

    public static float scaleFactor;
    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        cam = GetComponent<Camera>();
        float currentScreenWidth = Screen.width;
        scaleFactor = currentScreenWidth / referenceWidth;

        cam.orthographicSize /= scaleFactor;
    }
 

}
