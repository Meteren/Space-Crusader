using TMPro;
using UnityEngine;

public class UIManager : SingleTon<UIManager> 
{

    [SerializeField] private TextMeshProUGUI fpsText;

    [Header("Effect Selection Screen")]
    public GameObject effectSelectionScreen;

    public void SetFpsBar(float fps)
    {
        fpsText.text = $"FPS:{fps}";
    }

    public void ActivateSkillWindow() =>
        effectSelectionScreen.SetActive(!effectSelectionScreen.activeSelf);

}
