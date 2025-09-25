using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;


    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;
    public Weapon currentWeapon;
    public Transform firePoint;
    public SpriteRenderer Weaponsprite;
    public Transform weapontransform;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            speed *= Character.Speed;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");


        if (currentWeapon != null)
            currentWeapon.Tick(Time.deltaTime);

        if (Input.GetMouseButton(0) && currentWeapon != null)
        {
            Debug.Log("Called Bullet");
            // currentWeapon.TryShoot(firePoint.position, (Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.WorldToScreenPoint(firePoint.position)));
            currentWeapon.TryShoot(firePoint.position, firePoint.right);
        }


    }

    void FixedUpdate()
    {

        if (!GameManager.Instance.isLive)
            return;

        AimTowardMouse();

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        anim.SetFloat("Speed", inputVec.magnitude);


        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {

            if (!GameManager.Instance.isLive)
                return;

            GameManager.Instance.health -= Time.deltaTime * 10;
            GameManager.Instance.UpdateHeathUI();
            if (GameManager.Instance.health < 0)
            {
                for (int index = 2; index < transform.childCount; index++)
                {
                    transform.GetChild(index).gameObject.SetActive(false);
                }

                anim.SetTrigger("Dead");
                GameManager.Instance.GameOver();
            }
        }
    }


    void AimTowardMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorld - transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weapontransform.rotation = Quaternion.Euler(0, 0, angle);
    }


}

