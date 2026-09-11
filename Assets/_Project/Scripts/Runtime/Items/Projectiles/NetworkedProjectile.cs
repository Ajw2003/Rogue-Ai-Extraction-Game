using System.Collections;
using Interfaces;
using UnityEngine;

// Plain MonoBehaviour for now - becomes a PurrNet NetworkBehaviour once networking is wired up
// (Phase 3), which is out of scope here.
public class NetworkedProjectile : MonoBehaviour
{
    public float lifeTime = 3f;
    public int Damage;

    private void Start()
    {
        StartCoroutine(DelayedDestroy());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out IHealth monster))
        {
            monster.TakeDamage(Damage);
        }

        DestroySelf();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
