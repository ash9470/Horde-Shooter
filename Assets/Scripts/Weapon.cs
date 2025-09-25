using UnityEngine;

[CreateAssetMenu(menuName = "Horde/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public GameObject projectilePrefab;
    public Sprite Weaponsprite;

    public float fireRate = 10f;
    public float projectileSpeed = 10f;
    public int damage = 1;
    public float spread = 0f;
    public int burstCount = 1;
    public float burstDelay = 0.05f;

    public float cooldown = 0f;
    public void TryShoot(Vector2 origin, Vector2 rawDirection)
    {
        if (cooldown > 0)
            return;
        cooldown = 1f / fireRate;


        Vector2 shootDir = rawDirection.normalized;

        if (burstCount <= 1)
        {
            SpawnProjectile(origin, shootDir);
        }
        else
        {
            for (int i = 0; i < burstCount; i++)
            {
                SpawnProjectile(origin, shootDir);
            }
        }
    }





    void SpawnProjectile(Vector2 origin, Vector2 direction)
    {
        GameObject p = BulletPool.Instance.GetBullet();
        p.transform.position = origin;
        p.transform.rotation = Quaternion.identity;

        Projectile proj = p.GetComponent<Projectile>();
        proj.Initialize(direction.normalized * projectileSpeed, damage);
    }
    public void Tick(float dt)
    {
        if (cooldown > 0) cooldown -= dt;
    }
}
