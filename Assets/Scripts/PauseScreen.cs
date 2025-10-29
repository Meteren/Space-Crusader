using UnityEngine;

public class PauseScreen : MonoBehaviour
{


    [Header("Effect Grid Controller")]
    [SerializeField] private EffectGridController effectGridController;

  
    public void HandlePauseMenu(GameObject pauseIcon)
    {
        pauseIcon.SetActive(!pauseIcon.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);
        if(GameManager.instance.initPartGenerationProcess)
            GameManager.instance.initPartGenerationProcess = false;
        if (!effectGridController.gameObject.activeSelf)
        {
            GameManager.instance.PauseOrContinueGame(false);
        }
        else
        {
            if (SoundManager.instance.MusicSource.isPlaying)
            {
                SoundManager.instance.PauseMusic();
            }      
            else
                SoundManager.instance.UnPauseMusic();     

        }
            
    }

    public void NavigateToMainMenu()
        => GameManager.instance.InitSceneChange(0);


}
