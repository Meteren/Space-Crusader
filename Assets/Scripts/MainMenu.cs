using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lastReachedLevelText;


    private void Start()
    {
        GameManager.instance.currentLevelIndex = GameManager.instance.GetSavedLevel();
        lastReachedLevelText.text = $"Last Reached Level: {GameManager.instance.currentLevelIndex + 1}";
    }
    public void OnPressStart()
    {
        GameManager.instance.InitSceneChange(1);
    }


    public void OnPressExit()
    {
        Application.Quit();
    }
}
