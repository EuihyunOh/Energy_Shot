using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float speed = 5.0f;
    public float unitDuration = 1.0f;
    public int cost = 100;

    protected Transform muzzle;
    protected Rigidbody2D projectileRb2D;
    protected GameObject projectile;
    protected Animator animator;

    protected float dir;
    protected bool isFire = false;
    protected bool isCharging = false;
    protected float fireStart = 0;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        muzzle = GetComponentInChildren<Transform>();
        animator = GetComponent<Animator>();

        dir = transform.localScale.x > 0 ? 1.0f : -1.0f;
    }

    protected virtual void Update()
    {
        if (isFire)
        {
            UnitDurationCheck();
        }
    }

    protected virtual void Fire()
    {
        Vector3 trans = new Vector3(muzzle.position.x, muzzle.position.y, 0);
        projectile = Instantiate(projectilePrefab, trans, Quaternion.identity);
        projectileRb2D = projectile.GetComponent<Rigidbody2D>();
    }

    protected virtual void UnitDurationCheck()
    {
        if (Time.fixedTime > fireStart + unitDuration)
        {
            animator.SetTrigger("Destroy");
        }
    }
}
