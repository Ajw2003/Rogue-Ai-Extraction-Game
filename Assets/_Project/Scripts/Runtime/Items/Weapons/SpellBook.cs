using System.Collections;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    private float _fireRate;
    private float _minSpread;
    private float _maxSpread;
    private float _reloadTime;
    private float _projectileForce;
    private float _projectileSize;

    private int _damage;
    private int _numberOfProjectiles;

    private bool _isHoming;
    private bool _isReloading;

    private Collider[] _playerColliders;

    private GameObject _projectilePrefab;

    public SpellStats spellStats;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform shootPoint;

    private void Start()
    {
        AssignStats();
        _playerColliders = gameObject.GetComponentsInChildren<Collider>();
    }

    public void AssignStats()
    {
        _fireRate = spellStats.fireRate;
        _minSpread = spellStats.minSpread;
        _maxSpread = spellStats.maxSpread;
        _reloadTime = spellStats.reloadTime;
        _projectileForce = spellStats.projectileForce;
        _damage = spellStats.damage;
        _numberOfProjectiles = spellStats.numberOfProjectiles;
        _projectilePrefab = spellStats.projectilePrefab;
        _isHoming = spellStats.isHoming;
        _projectileSize = spellStats.projectileSize;
        _projectilePrefab.GetComponent<NetworkedProjectile>().lifeTime = spellStats.lifeTime;
        _projectilePrefab.GetComponent<NetworkedProjectile>().Damage = spellStats.damage;
        _projectilePrefab.transform.localScale = new Vector3(spellStats.projectileSize, spellStats.projectileSize, spellStats.projectileSize);
    }

    public void CastSpell()
    {
        if (_isReloading) return;
        _isReloading = true;

        StartCoroutine(ReloadRoutine());

        // Ray cast from screen center for aiming so the crosshair is always where the shot will go.
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 shootDirection = (targetPoint - shootPoint.position).normalized;
        int projectileCount = Mathf.Max(_numberOfProjectiles, 1);

        for (int i = 0; i < projectileCount; i++)
        {
            FireProjectile(shootDirection);
        }
    }

    private void FireProjectile(Vector3 baseDirection)
    {
        float currentSpread = Random.Range(_minSpread, _maxSpread);
        Vector3 spreadOffset = Random.insideUnitSphere * currentSpread;
        Vector3 finalDirection = (baseDirection + spreadOffset).normalized;

        var projectileInstance = Instantiate(_projectilePrefab, shootPoint.position, Quaternion.LookRotation(finalDirection));
        var projectileCollider = projectileInstance.GetComponent<Collider>();
        foreach (var playerCollider in _playerColliders)
        {
            Physics.IgnoreCollision(projectileCollider, playerCollider);
        }

        var projectileRb = projectileInstance.GetComponent<Rigidbody>();
        projectileRb.AddForce(_projectileForce * finalDirection, ForceMode.Impulse);
    }

    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(_reloadTime);
        _isReloading = false;
    }
}
