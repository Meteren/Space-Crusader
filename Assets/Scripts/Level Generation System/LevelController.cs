using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Level Controller Attributes")]
    [SerializeField] private List<Level> levels;
    [SerializeField] private Transform levelGenerationPoint;
    [SerializeField] private Transform movePoint;
    [SerializeField] private float levelPartMoveSpeed;

    int currentLevelIndex => GameManager.instance.currentLevelIndex;

    int partIndex = 0;

    PlayerController playerReference;

    List<Transform> createdLevelParts = new();
    void Start()
    {
        Transform levelPart = levels[currentLevelIndex].GeneratePart(partIndex);
        levelPart.position = levelGenerationPoint.position;

        createdLevelParts.Add(levelPart);
    }

    // Update is called once per frame
    void Update()
    {
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
                    Transform newLevelPart = levels[currentLevelIndex].GeneratePart(++partIndex);
                    newLevelPart.position = levelGenerationPoint.position;
                    createdLevelParts.Add(newLevelPart);
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
