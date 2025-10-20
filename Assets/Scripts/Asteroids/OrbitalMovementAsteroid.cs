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
    [SerializeField] private Transform centerPoint;

    float angle;
  
    protected override void Start()
    {
        base.Start();
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

        transform.position = centerPoint.position + new Vector3(x, y,0);

    }

    public int ConvertMoveSide() => side == OrbitMoveSide.Left ? -1 : 1;

}
