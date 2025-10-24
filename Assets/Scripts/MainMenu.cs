using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnPressStart()
    {
        GameManager.instance.InitSceneChange(1);
    }


    public void OnPressExit()
    {
        Application.Quit();
    }
}
