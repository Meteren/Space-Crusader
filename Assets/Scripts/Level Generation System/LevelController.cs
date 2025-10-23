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

    private Slider skillWindowProgressBar;
    private TextMeshProUGUI skillWindowProgressBarText;

    EffectGridController effectGridController;
    int currentLevelIndex => GameManager.instance.currentLevelIndex;

    public static int partIndex = 0;

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
    void Start()
    {
        effectGridController = FindFirstObjectByType<EffectGridController>();   
        skillWindowProgressBar = GameObject.Find("SkillWindowProgressBar").GetComponent<Slider>();
        skillWindowProgressBarText = skillWindowProgressBar.transform.Find("BottomText").GetComponent<TextMeshProUGUI>();
        Transform levelPart = levels[currentLevelIndex].GeneratePart(partIndex);
        AnchorGameObject anchor = levelGenerationPoint.gameObject.GetComponent<AnchorGameObject>();

        anchor.anchorType = AnchorGameObject.AnchorType.TopCenter;

        anchor.anchorOffset.y = 4f;

        anchor.UpdateAnchor();

        levelPart.position = levelGenerationPoint.position;

        createdLevelParts.Add(levelPart);
        partIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if(pierceSkillProgress == null)
            pierceSkillProgress = GameObject.Find("PiercerProgressBar").GetComponent<Slider>();

        skillWindowProgressBar.value = progressAmount;

        Debug.Log("Skill window progress bar value: " + skillWindowProgressBar.value);

        if(effectGridController.ActiveMaxedOutEffectCount != effectGridController.EffectCount)
        {
            if (skillWindowProgressBar.value >= 1)
            {
                skillWindowProgressBar.value = 0;
                progressAmount = 0;
                if (pierceSkillProgress != null)
                    pierceSkillProgress.value -= 0.02f;

                UIManager.instance.HandleSkillWindow();

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


        if (!playerReference)
            playerReference = GameObject.FindFirstObjectByType<PlayerController>();

        MoveLevelParts();
        TryDeleteAndCreateParts();
                    
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
                    InitPartGeneration();
            }

            if(part != null)
            {
               
                if (Vector2.Distance(part.position, playerReference.transform.position) <= generationRange)
                {
                    if(createdLevelParts.Count < 2)
                        InitPartGeneration();
                }
      
             
            }
        }
    }

    private void InitPartGeneration()
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
                //will be changed later with a proper UI declaration
            Debug.Log("No Level Part To Create!!!");
        }
        
       
    }

}

