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
    [Range(0.0f, 20.0f)] public float speed = 10.0f;
    
    

    //외부 파라미터
    public bool playerActive = true;
    

    //연동할 프리팹 등록
    public GameObject laserDefaultPrefab;
    public GameObject EnergyDefaultPrefab;

    //애니메이션 해시
    public readonly static int ANISTS_Move_Front = Animator.StringToHash("Base Layer.Player_Move_Front");
    public readonly static int ANISTS_Move_Back = Animator.StringToHash("Baase Layer.Player_Move_Back");

    //내부 파라미터
    protected Rigidbody2D rb2D;
    protected Animator animator;
    protected GameController gameController;

    protected int energy = 0;
    protected int genEnergyAmount = 0;
    protected int consumeAmount = 0;
    protected float energyGenTime = 0.0f;
    protected float timeCheck = 0.0f;
    

    protected virtual void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        energy = gameController.initEnergy;
        energyGenTime = gameController.energyGenTime;
        genEnergyAmount = gameController.genEnergyMount;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();        
    }

    //자원 UI에 업데이트
    protected void UpdateEnergy<T>(int change) where T : UI_Resource
    {
        int origin = energy;
        energy = Mathf.Clamp(energy + change, 0, 9999);
        FindObjectOfType<T>().UpdateNumber(origin, energy);
    }

    //자원 자동 획득
    protected void GenerateEnergy<T>() where T : UI_Resource
    {        
        if (timeCheck == 0.0f)
        {
            timeCheck = Time.fixedTime;
        }

        if (Time.fixedTime > timeCheck + energyGenTime)
        {
            timeCheck = 0.0f;
            UpdateEnergy<T>(genEnergyAmount);
        }
        
    }

    //자원 부족시 메세지 출력
    protected void WarnMessage<T>() where T : UI_Resource
    {
        FindObjectOfType<T>().UpdateMessage("Not enough Energy");
    }
    

    protected void ActionMove(float input_X, float input_Y)
    {
        //애니메이션 설정
        //이동
        if (input_X > 0.0f)
        {
            animator.SetTrigger("Move Front");
        }
        else if (input_X < 0.0f)
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
    protected virtual void LaserAttack(Skill skill)
    {
        switch (skill)
        {
            case Skill.Default:
                Instantiate(laserDefaultPrefab, transform.position, Quaternion.identity);
                //소비 자원량
                consumeAmount = laserDefaultPrefab.GetComponent<Laser_Default>().cost;
                break;
        }
    }

    protected virtual void EnergyAttack(Skill skill)
    {
        switch (skill)
        {
            case Skill.Default:
                Instantiate(EnergyDefaultPrefab, transform.position, Quaternion.identity);
                //소비 자원량
                consumeAmount = EnergyDefaultPrefab.GetComponent<Energy_Default>().cost;
                break;
        }
    }

    protected void DestroyObject()
    {
        Destroy(gameObject);
    }

    

    public int GetEnergy()
    {
        return energy;
    }

    public void EnergyConsume(int n)
    {
        energy -= n;
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
}
