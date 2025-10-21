using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Header("Level Controller Attributes")]
    [SerializeField] private List<Level> levels;
    [SerializeField] private Transform levelGenerationPoint;
    [SerializeField] private Transform movePoint;
    [SerializeField] private float levelPartMoveSpeed;
    [SerializeField] private Slider skillWindowProgressBar;
    int currentLevelIndex => GameManager.instance.currentLevelIndex;

    public static int partIndex = 0;

    //--skill windwow progress bar handlements

    public float progressAmount;

    float decreaseValueMultiplier = 1;

    float partIndexNormalization => Mathf.InverseLerp(0f, (float)levels[currentLevelIndex].LevelParts.Count, (float)partIndex);

    public float finalDecreaseVal => decreaseValueMultiplier * (1 - (partIndexNormalization == 1 ? 0.1f : partIndexNormalization));

    //--

    PlayerController playerReference;

    List<Transform> createdLevelParts = new();
    void Start()
    {
        skillWindowProgressBar = GameObject.Find("SkillWindowProgressBar").GetComponent<Slider>();
        Transform levelPart = levels[currentLevelIndex].GeneratePart(partIndex);
        levelPart.position = levelGenerationPoint.position;

        createdLevelParts.Add(levelPart);
        partIndex++;
    }

    // Update is called once per frame
    void Update()
    {

        skillWindowProgressBar.value = progressAmount;

        if (skillWindowProgressBar.value >= 1)
        {
            UIManager.instance.HandleSkillWindow();
            skillWindowProgressBar.value = 0;
            progressAmount = 0;
            decreaseValueMultiplier -= 0.2f;
            decreaseValueMultiplier = Mathf.Clamp(decreaseValueMultiplier,0.1f,1f);
        }

        if(!playerReference)
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
                if(partIndex < levels[currentLevelIndex].LevelParts.Count)
                {
                    Transform newLevelPart = levels[currentLevelIndex].GeneratePart(partIndex);
                    newLevelPart.position = levelGenerationPoint.position;
                    createdLevelParts.Add(newLevelPart);
                    partIndex++;
                }
                else
                {
                    //will be changed later with a proper UI declaration
                    Debug.Log("You finished the level!!!");
                }
               
            }
        }
    }

}

