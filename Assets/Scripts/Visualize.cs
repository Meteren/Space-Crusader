using UnityEngine;

public class Visualize : MonoBehaviour
{
    [SerializeField] private float radius;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);    
    }
}
