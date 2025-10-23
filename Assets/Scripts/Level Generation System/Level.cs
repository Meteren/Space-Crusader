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

            AnchorGameObject anchor = child.gameObject.GetComponent<AnchorGameObject>();

            anchor.anchorType = AnchorGameObject.AnchorType.MiddleCenter;
            anchor.anchorOffset.x = (child.position.x - (CameraViewportHandler.Instance.MiddleCenter.y));
            anchor.anchorOffset.y = child.position.y - CameraViewportHandler.Instance.MiddleCenter.y;

            anchor.anchorOffset.x *= (CameraViewportHandler.Instance.Width / (2 * CameraViewportHandler.Instance.referenceWidthPositiveX));

            anchor.UpdateAnchor();
    
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
