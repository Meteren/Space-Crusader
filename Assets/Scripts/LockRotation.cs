using UnityEngine;

public class LockRotation : MonoBehaviour
{
    Quaternion startRotation;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startRotation = rectTransform.rotation;
    }
    private void LateUpdate()
    {
        rectTransform.rotation = startRotation;
    }
}
