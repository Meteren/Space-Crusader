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
        float reference = referenceWidth;
        float currentScreenWidth = Screen.width;
        scaleFactor = currentScreenWidth / reference;

        cam.orthographicSize /= scaleFactor;
    }
 

}
