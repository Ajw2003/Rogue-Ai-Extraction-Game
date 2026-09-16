using UnityEngine;

[CreateAssetMenu(fileName = "SpellStats", menuName = "Scriptable Objects/SpellStats")]
public class SpellStats : ScriptableObject
{
    public float fireRate;
    public float minSpread;
    public float maxSpread;
    public float reloadTime;
    public float projectileForce;
    public float lifeTime;
    public float projectileSize;

    public int damage;
    public int numberOfProjectiles;

    public bool isHoming;

    public GameObject projectilePrefab;
}
