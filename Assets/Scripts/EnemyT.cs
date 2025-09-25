using System.Collections;
using UnityEngine;

public class EnemyT : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 2f;
    public float health;
    public float maxHealth = 3f;

    [Header("Components")]
    public Rigidbody2D target;
    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;

    bool isLive;
    WaitForFixedUpdate wait;

    [HideInInspector] public EnemySpawner spawner; // assigned by spawner when spawned

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        // Reset state each time it’s reused (from pooling)
        if (GameManager.Instance != null)
            target = GameManager.Instance.player.GetComponent<Rigidbody2D>();

        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isLive || !isLive) return;

        // Simple direction vector
        Vector2 dirVec = (target.position - rigid.position).normalized;

        // Use velocity instead of MovePosition + zeroing
        rigid.velocity = dirVec * speed;
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive || !isLive) return;

        spriter.flipX = target.position.x < rigid.position.x;
    }

    public void ResetEnemy()
    {
        health = maxHealth;
        isLive = true;
        rigid.velocity = Vector2.zero;
        coll.enabled = true;
        rigid.simulated = true;
        anim.SetBool("Dead", false);
        spriter.sortingOrder = 2;
    }

    public void TakeDamage(int damage)
    {
        if (!isLive) return;

        health -= damage;
        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            Die();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    void Die()
    {
        isLive = false;
        coll.enabled = false;
        rigid.simulated = false;
        spriter.sortingOrder = 1;
        anim.SetBool("Dead", true);

        GameManager.Instance.RegisterKill();
        StartCoroutine(RemoveEnemy());
    }

    WaitForSeconds Seconds = new WaitForSeconds(1);


    IEnumerator RemoveEnemy()
    {
        yield return Seconds;
        if (spawner != null)
            spawner.RemoveEnemy(gameObject);
        else
            gameObject.SetActive(false);

    }


}
