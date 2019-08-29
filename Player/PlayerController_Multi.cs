using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

using Photon.Pun;
using Photon.Realtime;

public enum Skill_List
{
    Default,
    Pattern,
}

public class PlayerController_Multi : MonoBehaviourPunCallbacks, IPunObservable
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
    public readonly static int ANISTS_IDLE = Animator.StringToHash("Base Layer.Player_Idle");
    public readonly static int ANISTS_MOVE_FRONT = Animator.StringToHash("Base Layer.Player_Move_Front");
    public readonly static int ANISTS_MOVE_BACK = Animator.StringToHash("Baase Layer.Player_Move_Back");

    //내부 파라미터
    protected Rigidbody2D rb2D;
    protected Animator animator;
    protected GameController gameController;
    protected Camera myCam;
    protected Tilemap map;

    protected int energy = 0;
    protected int genEnergyAmount = 0;
    protected int consumeAmount = 0;
    protected float energyGenTime = 0.0f;
    protected float timeCheck = 0.0f;
    protected bool isAlive = true;
    protected bool spawnAvailable = true;

    AnimatorStateInfo ANISTS;
    

    protected virtual void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        energy = gameController.initEnergy;
        energyGenTime = gameController.energyGenTime;
        genEnergyAmount = gameController.genEnergyMount;
        map = gameController.availableArea;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            //카메라 연결
            FindObjectOfType<CameraFollow>().target = transform;
        }
    }

    protected virtual void Update()
    {
        if (photonView.IsMine) // 내 캐릭터이면 작동
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            ActionMove(x, y);
            ANISTS = animator.GetCurrentAnimatorStateInfo(0);

            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Q))
            {
                if (energy < consumeAmount)
                {
                    //부족하면 경고 메세지
                    WarnMessage<Player_UI_Multi>();
                }
                else
                {
                    LaserAttack(Skill_List.Default);
                    //자원 소비
                    UpdateEnergy<Player_UI_Multi>(-consumeAmount);
                }
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.W))
            {
                if (energy < consumeAmount)
                {
                    //부족하면 경고 메세지
                    WarnMessage<Player_UI_Multi>();
                }
                else
                {
                    EnergyAttack(Skill_List.Default);
                    //자원 소비
                    UpdateEnergy<Player_UI_Multi>(-consumeAmount);
                }
            }
            if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.E))
            {

            }
        }
        else // 내 캐릭터가 아니라면 작동
        {
            //네비게이션 대상으로 지정
            FindObjectOfType<Player_UI_Multi>().enemyPos = transform.position;
            FindObjectOfType<Player_UI_Multi>().isEnemySet = true;

        }
    }

    protected void LateUpdate()
    {
        GenerateEnergy<Player_UI_Multi>();
    }

    //자원 UI에 업데이트
    protected void UpdateEnergy<T>(int change) where T : UI_Resource
    {
        if (!spawnAvailable)
        {
            spawnAvailable = true;
            return;
        }
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
    protected virtual void LaserAttack(Skill_List skill)
    {
        switch (skill)
        {
            case Skill_List.Default:
                SpawnWeapon<Laser_Default_Multi>(this.laserDefaultPrefab, Quaternion.identity);
                break;

            case Skill_List.Pattern:
                SpawnWeapon<Laser_Pattern_Multi>(laserPatternPrefab,  Quaternion.identity);
                break;
        }
    }

    //스킬 B : 에너지탄 공격
    protected virtual void EnergyAttack(Skill_List skill)
    {
        switch (skill)
        {
            case Skill_List.Default:
                SpawnWeapon<Energy_Default_Multi>(energyDefaultPrefab, Quaternion.identity);
                
                break;
        }
    }

    //무기 소환
    protected void SpawnWeapon<T>(GameObject prefab, Quaternion quat) where T : WeaponController_Multi
    {
        Vector3 spawnPoint = GetSpawnPoint();

        if (!spawnAvailable)
        {
            return;
        }
        GameObject ins = PhotonNetwork.Instantiate(prefab.name, spawnPoint, quat);
        ins.transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);

        T controller = ins.GetComponent<T>();
        //소비 자원량 입수
        consumeAmount = controller.cost;
        controller.SetOwner(tag);
    }

    //소환 타일 탐색
    protected Vector3 GetSpawnPoint()
    {
        try
        {
            Vector3Int pos = new Vector3Int(map.WorldToCell(transform.position).x, map.WorldToCell(transform.position).y, 0);
            spawnAvailable = gameController.OnFlag(pos);
            return new Vector2(map.GetCellCenterWorld(pos).x, map.GetCellCenterWorld(pos).y);      
            
        }
        catch (NullReferenceException)
        {
            Debug.Log("Map is not set");
        }

        return Vector3.zero;

    }

    //오브젝트 제거
    protected void DestroyObject()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }        
    }

    //Follow하는 camera 가져오기
    public void SetCamera(Camera cam)
    {
        myCam = cam;
    }

    //Energy 반환
    public int GetEnergy()
    {
        return energy;
    }

    //Energy 변화
    public void EnergyConsume(int n)
    {
        energy -= n;
    }

    //죽는 애니메이션 재생 및 관련 파라미터 조정
    public void Dead()
    {
        if (photonView.IsMine)
        {
            animator.SetTrigger("Dead");
            isAlive = false;
            rb2D.velocity = new Vector2();
        }        
    }


    
    // 기타 변수를 넘길때 사용
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {        
        
    }
}
