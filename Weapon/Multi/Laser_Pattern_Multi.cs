using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Pattern_Multi : WeaponController_Multi
{
    public float range = 200.0f;
    public float width = 0.001f;
    public float rotationSpeed = 5.0f;
    public float moveSpeed = 10.0f;

    BoxCollider2D[] boxList;
    Rigidbody2D rb2D;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        boxList = GetComponentsInChildren<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        //태그 설정
        foreach (BoxCollider2D box in boxList)
        {
            box.GetComponent<SpriteRenderer>().tag = tag;
        }

        // 회전 설정
        rb2D.AddTorque(rotationSpeed);

        //전진 설정
        rb2D.velocity = new Vector2(dir*moveSpeed, 0.0f);
        isFire = true;
        fireStart = Time.fixedTime;
        //Debug.Log("Laser Pattern");
        
    }
       
    protected override void Fire()
    {
        
    }

    protected override void UnitDurationCheck()
    {
        if (Time.fixedTime > fireStart + unitDuration)
        {            
            foreach(BoxCollider2D box in boxList)
            {
                box.enabled = false;
                box.GetComponent<SpriteRenderer>().enabled = false;
            }
            isFire = false;
            animator.SetTrigger("Destroy");
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
