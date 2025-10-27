using UnityEngine;
public class LinearMovingAsteroid : Asteroid
{

    [Header("Linear Movement Attributes")]
    [SerializeField] private Transform[] movePositions;
    [SerializeField] private float moveSpeed;

    int selectedPositionIndex;
    protected override void Update()
    {
        base.Update();
        MoveLogic();

    }
    public override void MoveLogic()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePositions[selectedPositionIndex].position, Time.deltaTime * moveSpeed);

        if (Vector2.Distance(transform.position, movePositions[selectedPositionIndex].position) < 0.01f)
        {
            selectedPositionIndex++;
            if (selectedPositionIndex >= movePositions.Length)
                selectedPositionIndex = 0;

        }
    }
}
