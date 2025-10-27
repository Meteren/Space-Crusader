using UnityEngine;

public class EndLevelScreen : MonoBehaviour
{

    public void MoveOnToNextLevel()
        => GameManager.instance.InitSceneChange(1);
    public void NavigateToMainMenu()
        =>
        GameManager.instance.InitSceneChange(0);

    private void OnEnable() => GameManager.instance.AdjustHighestScoreIfNeeded();


}
