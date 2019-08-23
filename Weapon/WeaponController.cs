using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //Inspector용
    public float range = 200.0f;
    public float fireDuration = 1.0f;
    public float initWidth = 0.1f;

    //내부파라미터
    LineRenderer laser;
    Animator animator;
    AnimationCurve originWidthCurve;
    float fireStart = 0;
    bool isCharging = false;
    bool isFire = false;
    

    //애니메이션 해시
    public readonly static int ANISTS_CHARGE = Animator.StringToHash("Base Layer.Laser_Charge");
    public readonly static int ANISTS_FULLCHARGE = Animator.StringToHash("Base Layer.Laser_FullCharge");

   
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponentInChildren<LineRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {         
        
    }

    // Update is called once per frame
    void Update()
    {

        DefaultAttack();

        //레이저 지속시간 끝나면 파기
        if (isFire)
        {
            if(Time.fixedTime > fireStart + fireDuration)
            {
                isFire = false;
                laser.enabled = false;
                animator.SetTrigger("Destroy");
            }
        }
    }

    //기본 공격
    void DefaultAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //Debug.Log(string.Format("{0}", stateInfo.fullPathHash));
        //Debug.Log(string.Format("{0}", ANISTS_CHARGE));
        //차지 확인
        if (stateInfo.fullPathHash == ANISTS_CHARGE)
        {
            if (!isCharging)
            {
                AnimationCurve curve = new AnimationCurve();
                curve.AddKey(0.0f, initWidth);
                curve.AddKey(1.0f, initWidth);
                originWidthCurve = laser.widthCurve; // 원래 너비 저장
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
        else if (stateInfo.fullPathHash == ANISTS_FULLCHARGE)
        {            
            if (!isFire)
            {
                //Debug.Log("Full charge");
                laser.widthCurve = originWidthCurve; // 원래대로 너비 변경                
                isFire = true;
                fireStart = Time.fixedTime;
            }
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
