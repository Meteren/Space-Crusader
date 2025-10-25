using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void Restart() => GameManager.instance.InitSceneChange(1);

    public void NavigateToMainMenu() => GameManager.instance.InitSceneChange(0);
}
