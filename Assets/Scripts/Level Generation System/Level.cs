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

            Vector2 childScreenPoint = Camera.main.WorldToScreenPoint(child.position);
            Vector2 partScreenPoint = Camera.main.WorldToScreenPoint(levelPart.position);

            float xDistanceToCenter = childScreenPoint.x - partScreenPoint.x;
            float yDistanceToCenter = childScreenPoint.y - partScreenPoint.y;

            Vector2 distance = new Vector2(xDistanceToCenter * CameraScaler.scaleFactorX,yDistanceToCenter * CameraScaler.scaleFactorY);

            float zDistance = Mathf.Abs(Camera.main.transform.position.z - child.position.z);

            childScreenPoint = partScreenPoint + distance;

            child.position = Camera.main.ScreenToWorldPoint(new Vector3(childScreenPoint.x,childScreenPoint.y,zDistance));

            if(child.gameObject.TryGetComponent<Asteroid>(out Asteroid asteroid))
            {
                int minHealth = (partIndex + 1) * partElementsHpBoostAmount;
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
