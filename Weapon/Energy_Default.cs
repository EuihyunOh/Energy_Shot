using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy_Default : WeaponController
{
    public float scale = 1.0f;

    //애니메이션 해시
    public readonly static int ANISTS_ENERGY_CHARGE = Animator.StringToHash("Base Layer.Energy_Charge");
    public readonly static int ANISTS_ENERGY_FULLCHARGE = Animator.StringToHash("Base Layer.Energy_FullCharge");

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.fullPathHash.Equals(ANISTS_ENERGY_CHARGE) || isFire)
        {
            return;
        }
        else if(stateInfo.fullPathHash.Equals(ANISTS_ENERGY_FULLCHARGE) && !isFire)
        {
            //Debug.Log("Shot!");
            Fire();
            isFire = true;
            fireStart = Time.fixedTime;
        }
    }

    protected override void Fire()
    {
        //Debug.Log("Energy Default Fire");
        base.Fire();
        projectile.transform.localScale = new Vector3(scale, scale, 1.0f);
        projectileRb2D.velocity = new Vector2(speed * dir, 0.0f);
        

    }
        
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
