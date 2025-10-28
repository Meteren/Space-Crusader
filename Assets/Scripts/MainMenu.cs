using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Texts")]
    [SerializeField] private TextMeshProUGUI lastReachedLevelText;
    [SerializeField] private TextMeshProUGUI highestScoreReached;

    [Header("Background Music")]
    [SerializeField] private AudioClip menuMusic;

    private void Start()
    {
        SoundManager.instance.PlayMusic(menuMusic,1f);
        GameManager.instance.highestScore = GameManager.instance.GetSavedScore();
        GameManager.instance.currentLevelIndex = GameManager.instance.GetSavedLevel();
        lastReachedLevelText.text = $"Last Reached Level: {GameManager.instance.currentLevelIndex + 1}";
        highestScoreReached.text = $"Highest Score: {GameManager.instance.highestScore}";
    }
    public void OnPressStart()
    {
        GameManager.instance.InitSceneChange(1);
        
    }

    public void OnPressRestart() => GameManager.instance.RestartGame();
}
