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
    // Update is called once per frame
    private void LateUpdate()
    {
        rectTransform.rotation = startRotation;
    }
}
