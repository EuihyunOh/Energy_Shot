using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Default : WeaponController
{
    //Inspector용
    public float range = 200.0f;
    public float initWidth = 0.1f;
    public float finalWidth = 0.4f;

    //내부파라미터
    LineRenderer laser;
    BoxCollider2D boxCol;    
    

    //애니메이션 해시
    public readonly static int ANISTS_LASER_CHARGE = Animator.StringToHash("Base Layer.Laser_Charge");
    public readonly static int ANISTS_LASER_FULLCHARGE = Animator.StringToHash("Base Layer.Laser_FullCharge");


    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        laser = GetComponentInChildren<LineRenderer>();
        boxCol = GetComponentInChildren<BoxCollider2D>();

        //태그 설정
        laser.tag = tag;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        Fire();        
    }

    //기본 공격
    override protected void Fire()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log(string.Format("{0}", stateInfo.fullPathHash));
        //Debug.Log(string.Format("{0}", ANISTS_CHARGE));
        //차지 확인
        if (stateInfo.fullPathHash == ANISTS_LASER_CHARGE)
        {
            if (!isCharging)
            {
                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0.0f, initWidth);
                curve.AddKey(1.0f, initWidth);
                laser.widthCurve = curve;                // 얇게 나가는 연출

                //레이저 위치 설정
                Vector2 pos = new Vector2(transform.position.x + range, 0.0f);
                laser.SetPosition(1, pos);
            }
            isCharging = true;            
            //Debug.Log("Charging");
            return;
        }
        //풀차지
        else if (stateInfo.fullPathHash == ANISTS_LASER_FULLCHARGE)
        {            
            if (!isFire)
            {
                //Debug.Log("Full charge");
                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0.0f, finalWidth);
                curve.AddKey(1.0f, finalWidth);
                laser.widthCurve = curve; // 풀차지 너비 변경                
                isFire = true;
                fireStart = Time.fixedTime;

                //collider 설정
                boxCol.size = new Vector2(range, finalWidth);
                boxCol.offset = new Vector2(range/2.0f, 0);

            }
        }
    }

    override protected void UnitDurationCheck()
    {
        if (isFire)
        {
            if (Time.fixedTime > fireStart + unitDuration)
            {
                isFire = false;
                laser.enabled = false;
                animator.SetTrigger("Destroy");
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
