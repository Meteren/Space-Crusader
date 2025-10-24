using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager> 
{

    [SerializeField] private TextMeshProUGUI fpsText;

    [Header("Effect Selection Screen")]
    public GameObject effectSelectionScreen;

    public void SetFpsBar(float fps)
    {
        fpsText.text = $"FPS:{fps}";
    }       

}
