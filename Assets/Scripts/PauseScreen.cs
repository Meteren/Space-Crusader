using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public void HandlePauseMenu(GameObject pauseIcon)
    {
        pauseIcon.SetActive(!pauseIcon.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
        GameManager.instance.PauseOrContinueGame();
    }

    public void NavigateToMainMenu()
        => GameManager.instance.InitSceneChange(0);
    

}
