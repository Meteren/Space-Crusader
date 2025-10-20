using UnityEngine;

public class OrbitalMovementAsteroid : Asteroid
{
    enum OrbitMoveSide
    {
        Left, Right
    }

    [Header("Orbital Movement Attributes")]
    [SerializeField] private float radius;
    [SerializeField] private OrbitMoveSide side;
    [SerializeField] private float orbitSpeed;

    float angle;
    Vector2 centerPos;
    protected override void Start()
    {
        base.Start();
        centerPos = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        MoveLogic();

    }

    public override void MoveLogic()
    {
        base.MoveLogic();
        angle += Time.deltaTime * orbitSpeed;

        float x = Mathf.Cos(angle * ConvertMoveSide()) * radius;

        float y = Mathf.Sin(angle * ConvertMoveSide()) * radius;    

        transform.position = centerPos + new Vector2(x, y);

    }

    public int ConvertMoveSide() => side == OrbitMoveSide.Left ? -1 : 1;

}
