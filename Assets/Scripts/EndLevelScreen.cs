using UnityEngine;

public class EndLevelScreen : MonoBehaviour
{

    public void MoveOnToNextLevel()
        => GameManager.instance.InitSceneChange(1);
    public void NavigateToMainMenu()
        =>
        GameManager.instance.InitSceneChange(0);

    public void EnableOrDisable() => gameObject.SetActive(!gameObject.activeSelf);

}
