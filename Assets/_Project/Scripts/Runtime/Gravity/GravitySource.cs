using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public enum GravityType { Spherical, Directional }

    // Self-registering registry: every enabled GravitySource adds itself here and removes itself
    // on disable, so resolvers (GravityReceiver, etc.) never need FindObjectsByType or a per-frame
    // scene search.
    private static readonly List<GravitySource> _activeSources = new();
    public static IReadOnlyList<GravitySource> ActiveSources => _activeSources;

    [Header("Settings")]
    public GravityType type = GravityType.Spherical;
    public float gravityStrength = 9.81f;
    public float influenceRadius = 50f;

    [Header("Directional Settings")]
    [Tooltip("The direction of gravity in local space of this object (usually 0, -1, 0)")]
    public Vector3 localGravityDirection = Vector3.down;

    private void OnEnable()
    {
        _activeSources.Add(this);
    }

    private void OnDisable()
    {
        _activeSources.Remove(this);
    }

    public Vector3 GetGravityDirection(Vector3 position)
    {
        if (type == GravityType.Spherical)
        {
            return (transform.position - position).normalized;
        }

        return transform.TransformDirection(localGravityDirection).normalized;
    }

    public float GetDistance(Vector3 position)
    {
        return Vector3.Distance(transform.position, position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, influenceRadius);

        if (type == GravityType.Directional)
        {
            Vector3 worldDir = transform.TransformDirection(localGravityDirection);
            Gizmos.DrawRay(transform.position, worldDir * 5f);
        }
    }
}
