using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public ParticleSpawner pSpawner;
    public LevelController levelController;
    private void Start()
    {

        DontDestroyOnLoad(gameObject);

        panelAnimator = panel.GetComponent<Animator>(); 
        panel.SetActive(false);

        SceneManager.sceneLoaded += OnSceneLoaded;


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
            pSpawner = FindFirstObjectByType<ParticleSpawner>();
            levelController = GameObject.Find("LevelGeneration").GetComponent<LevelController>();

        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        CalculateAvarageFpsAndShow();
#endif
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
        if(!isGamePaused)
            SoundManager.instance.StopMusicSmoothly(fadeInDuration);
        yield return new WaitForSecondsRealtime(fadeInDuration);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);

        while(asyncOperation.progress < 0.9)
            yield return null;

        yield return new WaitWhile(() => !asyncOperation.isDone);

        yield return null;

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
            PauseOrContinueGame(false);

    }

    public void PauseOrContinueGame(bool isActivatedBySkillWindow)
    {       

        if (Time.timeScale == 0)
        {
            if (!isActivatedBySkillWindow)
            {
                SoundManager.instance.UnPauseMusic();
            }
            SoundManager.instance.UnPauseSFX();
            Time.timeScale = 1f;
            isGamePaused = false;
        }
        else
        {
            if (!isActivatedBySkillWindow)
            {
                SoundManager.instance.PauseMusic();
            }

            SoundManager.instance.PauseSFX();
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
