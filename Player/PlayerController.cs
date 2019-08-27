using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skill
{
    Default,
    Pattern,
}

public class PlayerController : MonoBehaviour
{
    //스탯
    [Range(0.0f, 20.0f)] public float speed = 10.0f;
    
    

    //외부 파라미터
    public bool playerActive = true;
    

    //연동할 프리팹 등록
    public GameObject laserDefaultPrefab;
    public GameObject laserPatternPrefab;
    public GameObject energyDefaultPrefab;

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
    protected bool isAlive = true;
    

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
        //생사 확인
        if (!isAlive)
        {
            return;
        }
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
                SpawnWeapon<Laser_Default>(laserDefaultPrefab, transform.position, Quaternion.identity);
                break;

            case Skill.Pattern:
                SpawnWeapon<Laser_Pattern>(laserPatternPrefab, transform.position, Quaternion.identity);
                break;
        }
    }

    //스킬 B : 에너지탄 공격
    protected virtual void EnergyAttack(Skill skill)
    {
        switch (skill)
        {
            case Skill.Default:
                SpawnWeapon<Energy_Default>(energyDefaultPrefab, transform.position, Quaternion.identity);
                
                break;
        }
    }

    //무기 소환
    protected void SpawnWeapon<T>(GameObject prefab, Vector3 pos, Quaternion quat) where T : WeaponController
    {
        GameObject ins = Instantiate(prefab, pos, quat);
        ins.transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);

        T controller = ins.GetComponent<T>();
        //소비 자원량 입수
        consumeAmount = controller.cost;
        controller.SetOwner(tag);
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
        isAlive = false;
        rb2D.velocity = new Vector2();
    }
}
