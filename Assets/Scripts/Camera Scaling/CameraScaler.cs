using UnityEngine;
using UnityEngine.UI;

public class CameraScaler : MonoBehaviour
{
    private GameObject canvas;
    [SerializeField] private float referenceWidth;
    [SerializeField] private float referenceHeight;

    Camera cam;

    public static float scaleFactorX;
    public static float scaleFactorY;

    public static float scaledRatio;

    public static float cameraTopWorldPos;
    public static float cameraBottomWorldPos;
    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
        cam = GetComponent<Camera>();

        cameraBottomWorldPos = cam.ScreenToWorldPoint(new Vector2(0,0)).y;

        cameraTopWorldPos = cam.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;

        float currentScreenWidth = Screen.width;
        float currentScreenHeight = Screen.height;

        Debug.Log($"Screen Width: {Screen.width}");
        Debug.Log($"Screen Height:{Screen.height}");

        scaleFactorX = currentScreenWidth / referenceWidth;
        scaleFactorY = currentScreenHeight / referenceHeight;

        float referenceAspect = referenceWidth / referenceHeight;
        float currentAspect = currentScreenWidth / currentScreenHeight;

        scaledRatio = currentAspect / referenceAspect;
        //cam.orthographicSize /= scaledRatio;

    }

    private void Update()
    {
        /*Debug.Log($"Camera Scale Factor X:{scaleFactorX}");
        Debug.Log($"Camera Scale Factor Y:{scaleFactorY}");

        Debug.Log($"Scale Final: {scaledRatio}");

        Debug.Log($"Camera top: {cameraTopWorldPos}");
        Debug.Log($"Camera bottom: {cameraBottomWorldPos}");*/
    }

}
