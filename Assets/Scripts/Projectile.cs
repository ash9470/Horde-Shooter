using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 1;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 velocity, int dmg)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
        damage = dmg;

        // Rotate sprite to match velocity direction
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Restart lifetime coroutine
        StopAllCoroutines();
        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        if (gameObject.activeSelf)
            BulletPool.Instance.ReturnBullet(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyT e = other.GetComponent<EnemyT>();
            if (e != null) e.TakeDamage(damage);

            // Instead of Destroy, return to pool
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }
}
