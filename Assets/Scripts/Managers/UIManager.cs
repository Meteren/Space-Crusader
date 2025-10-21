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

    public void HandleSkillWindow()
    {
        effectSelectionScreen.SetActive(!effectSelectionScreen.activeSelf);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
            GameManager.instance.isGamePaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            GameManager.instance.isGamePaused = true;
        }
            

    }
        

}
