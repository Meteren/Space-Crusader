using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("Level Controller Attributes")]
    [SerializeField] private List<Level> levels;
    [SerializeField] private Transform levelGenerationPoint;
    [SerializeField] private Transform movePoint;
    [SerializeField] private float levelPartMoveSpeed;

    public int LevelCount { get => levels.Count; }

    private Slider skillWindowProgressBar;
    private TextMeshProUGUI skillWindowProgressBarText;

    private Slider levelProgressBar;

    EffectGridController effectGridController;
    int currentLevelIndex => GameManager.instance.currentLevelIndex;

    public int partIndex = 0;

    //--skill windwow progress bar handlements

    [HideInInspector] public float progressAmount;

    float decreaseValueMultiplier = 1;

    float generationRange = 1f;

    float partIndexNormalization => Mathf.InverseLerp(0f, (float)levels[currentLevelIndex].LevelParts.Count, (float)partIndex);

    public float finalDecreaseVal => decreaseValueMultiplier * (1 - (partIndexNormalization == 1 ? 0.1f : partIndexNormalization));

    //--

    PlayerController playerReference;

    Slider pierceSkillProgress;

    List<Transform> createdLevelParts = new();

    //--
    float stopAt;

    float levelBarInceraseSpeed = 0.2f;
    //--

    //--End level menu

    private GameObject endLevelScreen;
    private GameObject gameOverScreen;

    //--Indicator
    private GameObject levelIndicator;
    private Animator levelIndicatorAnimator;
    float indicatorMoveCenterDuration;
    float indicatorMoveRightDuration;
    bool generationStarted;

    bool isLevelFinished;

    void Start()
    {
        effectGridController = FindFirstObjectByType<EffectGridController>();   

        skillWindowProgressBar = GameObject.Find("SkillWindowProgressBar").GetComponent<Slider>();
        skillWindowProgressBarText = skillWindowProgressBar.transform.Find("BottomText").GetComponent<TextMeshProUGUI>();

        levelProgressBar = GameObject.Find("LevelProgressBar").GetComponent<Slider>();

        endLevelScreen = GameObject.Find("LevelEndScreen");
        levelIndicator = GameObject.Find("LeveIndicator");
        gameOverScreen = GameObject.Find("GameOverScreen");
        levelIndicator.SetActive(false);
        endLevelScreen.SetActive(false);
        gameOverScreen.SetActive(false);

        levelIndicatorAnimator = levelIndicator.GetComponent<Animator>();  

        RuntimeAnimatorController rtController = levelIndicatorAnimator.runtimeAnimatorController;

        foreach(var clip in rtController.animationClips)
        {
            if (clip.name == "slideToCenter")
                indicatorMoveCenterDuration = clip.length;
            if (clip.name == "slideToRight")
                indicatorMoveRightDuration = clip.length;
        }

        
        AnchorGameObject anchor = levelGenerationPoint.gameObject.GetComponent<AnchorGameObject>();

        anchor.anchorType = AnchorGameObject.AnchorType.TopCenter;

        anchor.anchorOffset.y = 4f;

        anchor.UpdateAnchor();    

    }

    void Update()
    {

        if (!playerReference)
            playerReference = GameObject.FindFirstObjectByType<PlayerController>();

        if (playerReference)
        {
            if (pierceSkillProgress == null)
                pierceSkillProgress = GameObject.Find("PiercerProgressBar").GetComponent<Slider>();

            if (GameManager.instance.initPartGenerationProcess && !generationStarted)
            {
                generationStarted = true;
                WaitForPartGeneration();
            }

            skillWindowProgressBar.value = progressAmount;

            Debug.Log("Skill window progress bar value: " + skillWindowProgressBar.value);

            if (effectGridController.ActiveMaxedOutEffectCount != effectGridController.EffectCount)
            {
                if (skillWindowProgressBar.value >= 1)
                {
                    skillWindowProgressBar.value = 0;
                    progressAmount = 0;
                    if (pierceSkillProgress != null)
                        pierceSkillProgress.value -= 0.02f;

                    effectGridController.HandleSkillWindow();

                    decreaseValueMultiplier -= 0.2f;

                    decreaseValueMultiplier -= 0.001f;

                    Debug.Log("DecreaseValue Mult: " + decreaseValueMultiplier);
                    decreaseValueMultiplier = Mathf.Clamp(decreaseValueMultiplier, 0.1f, 1f);
                }
            }
            else
            {
                Debug.Log("All skills are achieved");
                skillWindowProgressBarText.gameObject.SetActive(true);
                skillWindowProgressBar.value = 1f;
            }


            IncreaseLevelBar();
            MoveLevelParts();
            TryDeleteAndCreateParts();
        }

        CheckIfGameOver();


    }

    private void CheckIfGameOver()
    {
        if(!playerReference)
            gameOverScreen.SetActive(true);

    }

    private void MoveLevelParts()
    {
        foreach(var part in createdLevelParts)
        {
            Debug.Log($"Moving the part: {part.name}");
            part.position = Vector2.MoveTowards(part.position, movePoint.position, Time.deltaTime * levelPartMoveSpeed);
        }
    }

    private void TryDeleteAndCreateParts()
    {
        foreach(var part in createdLevelParts.ToList())
        {
            int asteroidCount = 0;
            for (int i = 0; i < part.childCount; i++)
            {
                if(part.GetChild(i).gameObject.GetComponent<Asteroid>())
                    asteroidCount++;
            }

            if (asteroidCount == 0)
            {
                Destroy(part.gameObject);
                createdLevelParts.Remove(part);
                if(createdLevelParts.Count == 0)
                    InitPartGeneration(true);
            }

            if(part != null)
            {
               
                if (Vector2.Distance(part.position, playerReference.transform.position) <= generationRange)
                {
                    if(createdLevelParts.Count < 2)
                        InitPartGeneration(false);
                }
      
             
            }
        }
    }

    private void InitPartGeneration(bool generateIfNoAsteroidsLeft)
    {
       
        if (partIndex < levels[currentLevelIndex].LevelParts.Count)
        {
            Transform newLevelPart = levels[currentLevelIndex].GeneratePart(partIndex);
            newLevelPart.position = levelGenerationPoint.position;
            createdLevelParts.Add(newLevelPart);
            partIndex++;
        }
        else
        {

            if (generateIfNoAsteroidsLeft)
            {
                partIndex++;
                Debug.Log("Level Finished");
            }
        }
        
       
    }

    private void IncreaseLevelBar()
    {
        float levelPartCount = (float)levels[currentLevelIndex].LevelParts.Count;

        float amountBetween = 1 / levelPartCount;

        stopAt = (partIndex - 1) * amountBetween;

        if(levelProgressBar.value < stopAt)
        {
            levelProgressBar.value += Time.deltaTime * levelBarInceraseSpeed;
        }

        stopAt = Mathf.Clamp(levelProgressBar.value, 0, stopAt);

        if (levelProgressBar.value == 1f && !isLevelFinished)
        {
            endLevelScreen.SetActive(true);
            isLevelFinished = true;

            if (currentLevelIndex == levels.Count - 1)
            {
                TextMeshProUGUI endLevelText = endLevelScreen.GetComponentInChildren<TextMeshProUGUI>();
                endLevelText.text = "Game Over";
                endLevelText.transform.Find("Next").GetComponent<Button>().enabled = false;
            }
            else
            {
                GameManager.instance.currentLevelIndex++;
                GameManager.instance.SaveLevelProgress();
               
            }
           
        }
            

    }

    private void WaitForPartGeneration()
    {
        StartCoroutine(WaitForLevelIndicatorRoutine());
    }


    private IEnumerator WaitForLevelIndicatorRoutine()
    {
        levelIndicator.SetActive(true);
        levelIndicator.GetComponent<TextMeshProUGUI>().text = $"Level {currentLevelIndex + 1}";
        levelIndicatorAnimator.SetBool("slideToCenter", true);
        yield return new WaitForSeconds(indicatorMoveCenterDuration + 0.5f);
        levelIndicatorAnimator.SetBool("slideToRight", true);
        yield return new WaitForSeconds(indicatorMoveRightDuration);
        levelIndicator.SetActive(false);
        StartLevelGeneration();
    }
    private void StartLevelGeneration()
    {
        Transform levelPart = levels[currentLevelIndex].GeneratePart(partIndex);
        levelPart.position = levelGenerationPoint.position;

        createdLevelParts.Add(levelPart);
        partIndex++;
        GameManager.instance.initPartGenerationProcess = false;
    }
 
}

