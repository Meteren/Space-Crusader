using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : SingleTon<GameManager>
{
    float elapsedTime = 0;
    float interval = 1f;
    int frameCount = 0;
    float fps;

    public int currentLevelIndex;

    [SerializeField] private PlayerSpawner playerSpawner;

    public bool isGamePaused;

    [Header("Fade-In and Out Segment")]
    [SerializeField] private GameObject panel;
    [SerializeField] private float waitFor;
    [HideInInspector] public Animator panelAnimator;
    float fadeInDuration;
    float fadeOutDuration;


    public bool initPartGenerationProcess;

    public float highestScore;

    public float scoreInALevel;
    private void Start()
    {

        DontDestroyOnLoad(gameObject);

        panelAnimator = panel.GetComponent<Animator>(); 
        panel.SetActive(false);

        SceneManager.sceneLoaded += OnSceneLoaded;

        //use it for testing
        //playerSpawner.Spawn();

        RuntimeAnimatorController rtController = panelAnimator.runtimeAnimatorController;

        foreach(var clip in rtController.animationClips)
        {
            if (clip.name == "fadeIn")
                fadeInDuration = clip.length;
            if(clip.name == "fadeOut")
                fadeOutDuration = clip.length;
        }


    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        ResetScore();
        if (scene.buildIndex != 0)
        {
            playerSpawner.Spawn();

        }
    }

    private void Update()
    {
        
        CalculateAvarageFpsAndShow();
    }

    private void CalculateAvarageFpsAndShow()
    {
        elapsedTime += Time.unscaledDeltaTime;
        frameCount++;
        fps += 1.0f / Time.deltaTime;
        if (elapsedTime >= interval)
        {
            UIManager.instance.SetFpsBar(Mathf.Floor((fps / frameCount) * 100) / 100);
            elapsedTime = 0;
            frameCount = 0;
            fps = 0;
        }
       
    }

    public void InitSceneChange(int sceneIndex)
    {
        StartCoroutine(SceneChangeRoutine(sceneIndex));
    }

    private IEnumerator SceneChangeRoutine(int sceneIndex)
    {
        panel.SetActive(true);
        panelAnimator.SetBool("fadeIn", true);
        yield return new WaitForSecondsRealtime(fadeInDuration);
        ChangeSceneTo(sceneIndex);
        yield return new WaitForSecondsRealtime(waitFor);
        panelAnimator.SetBool("fadeOut", true);
        yield return new WaitForSecondsRealtime(fadeOutDuration);
        panelAnimator.SetBool("fadeIn", false);
        panelAnimator.SetBool("fadeOut", false);

      
        Scene scene = SceneManager.GetActiveScene();

        if (scene.buildIndex == 1)
        {
            initPartGenerationProcess = true;
            panelAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }else
            panelAnimator.updateMode = AnimatorUpdateMode.Normal;

        panel.SetActive(false);

        if (isGamePaused)
            PauseOrContinueGame();

    }

    private void ChangeSceneTo(int sceneIndex) => 
        SceneManager.LoadSceneAsync(sceneIndex);


    public void PauseOrContinueGame()
    {

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
            isGamePaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            isGamePaused = true;
        }
    }

    public void SaveLevelProgress()
    {
        PlayerPrefs.SetInt("LevelIndex", currentLevelIndex);
        PlayerPrefs.Save();
    }

    public void SaveHighestScore()
    {
        PlayerPrefs.SetFloat("HighestScore", highestScore);
        PlayerPrefs.Save();
    }

    public int GetSavedLevel()
    {
        return PlayerPrefs.GetInt("LevelIndex", 0);
    }

    public float GetSavedScore()
    {
        return PlayerPrefs.GetFloat("HighestScore", 0);
    }


    public void AdjustHighestScoreIfNeeded()
    {
        if (scoreInALevel > highestScore)
        {
            highestScore = scoreInALevel;
            SaveHighestScore();
        }
    }

    public void ResetScore() => scoreInALevel = 0;

    public void RestartGame()
    {
        currentLevelIndex = 0;
        SaveLevelProgress();
        InitSceneChange(1);

    }


}
