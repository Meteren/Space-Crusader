using TMPro;
using UnityEngine;

public class UIManager : SingleTon<UIManager> 
{

    [SerializeField] private TextMeshProUGUI fpsText;

    public void SetFpsBar(float fps)
    {
        fpsText.text = $"FPS:{fps}";
    }
}
