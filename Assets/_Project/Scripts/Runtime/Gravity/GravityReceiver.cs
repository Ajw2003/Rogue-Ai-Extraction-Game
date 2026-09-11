using System.Collections.Generic;
using UnityEngine;

// Resolves the dominant GravitySource for this body, drives it with a physics force instead of
// Unity's built-in gravity, and exposes the resulting "up" so gameplay code can move and orient
// relative to whatever planet currently owns this body.
[RequireComponent(typeof(Rigidbody))]
public class GravityReceiver : MonoBehaviour
{
    [SerializeField] private float _alignmentSpeed = 10f;

    private Rigidbody _rb;
    private GravitySource _dominantSource;
    private Vector3 _up = Vector3.up;

    public Vector3 Up => _up;
    public GravitySource DominantSource => _dominantSource;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        // Rotation is owned by this component and by look input, never by the solver.
        // See docs/systems/gravity.md - Invariants.
        _rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        ResolveDominantGravitySource();
        ApplyGravityForce();
        AlignToGravityUp();
    }

    private void ResolveDominantGravitySource()
    {
        IReadOnlyList<GravitySource> sources = GravitySource.ActiveSources;
        float closestDistance = float.MaxValue;
        GravitySource closest = null;

        for (int i = 0; i < sources.Count; i++)
        {
            GravitySource source = sources[i];
            float distance = source.GetDistance(transform.position);
            if (distance < source.influenceRadius && distance < closestDistance)
            {
                closestDistance = distance;
                closest = source;
            }
        }

        // Keep the previous source when nothing is currently in range, rather than snapping
        // "up" back to world-up, so a body coasting out of every influence radius doesn't
        // suddenly reorient mid-flight.
        if (closest != null)
        {
            _dominantSource = closest;
        }
    }

    private void ApplyGravityForce()
    {
        if (_dominantSource == null) return;

        Vector3 gravityDirection = _dominantSource.GetGravityDirection(transform.position);
        _up = -gravityDirection;
        _rb.AddForce(gravityDirection * _dominantSource.gravityStrength, ForceMode.Acceleration);
    }

    private void AlignToGravityUp()
    {
        if (_dominantSource == null) return;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, _up) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _alignmentSpeed * Time.fixedDeltaTime);
    }
}
