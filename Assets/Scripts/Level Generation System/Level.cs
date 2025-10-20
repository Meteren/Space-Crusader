using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Level
{
    [SerializeField] private List<GameObject> levelParts;

    [SerializeField] private int partElementsHpBoostAmount;

    public List<GameObject> LevelParts {  get { return levelParts; } }

    private int minHealthStartPoint;

    private bool startPointInitted;

    public Transform GeneratePart(int partIndex)
    {

        GameObject instantiatedGO = GameObject.Instantiate(levelParts[partIndex]);

        Transform levelPart = instantiatedGO.transform;

        for (int i = 0; i < levelPart.childCount; i++)
        {
            Transform child = levelPart.GetChild(i).transform; 

            float xDistanceToCenter = child.transform.position.x - levelPart.position.x;
            float yDistanceToCenter = child.transform.position.y - levelPart.position.y;

            xDistanceToCenter /= CameraScaler.scaleFactor;
            yDistanceToCenter /= CameraScaler.scaleFactor;

            child.transform.position = levelPart.position + new Vector3(xDistanceToCenter, yDistanceToCenter, 0);

            if(child.gameObject.TryGetComponent<Asteroid>(out Asteroid asteroid))
            {
                int minHealth = (partIndex + 1 + GameManager.instance.currentLevelIndex) * partElementsHpBoostAmount;
                if (!startPointInitted)
                {
                    startPointInitted = true;
                    minHealthStartPoint = minHealth;
                }
                int randomValueForHealth = UnityEngine.Random.Range(minHealthStartPoint, minHealth * 3);
                asteroid.health = randomValueForHealth;
            }
                
        }

        return levelPart;  

    }
}
