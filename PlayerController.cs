using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skill
{
    Default,
}
public class PlayerController : MonoBehaviour
{
    //스탯
    [Range(0.0f, 20.0f)]public float speed = 10.0f;

    //외부 파라미터
    public bool playerActive = true;

    //연동할 프리팹 등록
    public GameObject laserDefaultPrefab;
    public GameObject EnergyDefaultPrefab;

    //애니메이션 해시
    public readonly static int ANISTS_Move_Front = Animator.StringToHash("Base Layer.Player_Move_Front");
    public readonly static int ANISTS_Move_Back = Animator.StringToHash("Baase Layer.Player_Move_Back");

    //내부 파라미터
    Rigidbody2D rb2D;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //키 입력
        float conMove_Vertical = Input.GetAxis("Vertical");
        float conMove_Horizontal = Input.GetAxis("Horizontal");
        ActionMove(conMove_Horizontal, conMove_Vertical);

        //Fire 1 : 레이저 공격
        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("Fire1");
            LaserAttack(Skill.Default);
        }

        //Fire 2 : 에너지 공격
        if (Input.GetButtonDown("Fire2"))
        {
            //Debug.Log("Fire2");
            EnergyAttack(Skill.Default);
        }
        
    }

    void ActionMove(float input_X, float input_Y)
    {
        //애니메이션 설정
        //이동
        if(input_X > 0.0f)
        {
            animator.SetTrigger("Move Front");
        }
        else if(input_X < 0.0f)
        {
            animator.SetTrigger("Move Back");
        }
        else
        {
            animator.SetTrigger("Idle");
        }    
        

        //이동 
        float velocity_X = input_X * speed;
        float velocity_Y = input_Y * speed;

        rb2D.velocity = new Vector2(velocity_X, velocity_Y);

        
    }

    

    //스킬 A : 레이저 공격
    void LaserAttack(Skill skill)
    {
        switch (skill) {
           case Skill.Default :
                Instantiate(laserDefaultPrefab, transform.position, Quaternion.identity);
            break;
        }
    }

    void EnergyAttack(Skill skill)
    {
        switch (skill)
        {
            case Skill.Default:
                Instantiate(EnergyDefaultPrefab, transform.position, Quaternion.identity);
                break;
        }
    }

    //주금
    void Dead()
    {

    }
}
