using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : PlayerController
{
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();

        //키 입력

        //float conMove_Vertical = Input.GetAxis("Joystick1 Vertical");
        float conMove_Vertical = Input.GetAxis("Vertical");
        //float conMove_Horizontal = Input.GetAxis("Joystick1 Horizontal");
        float conMove_Horizontal = Input.GetAxis("Horizontal");

        ActionMove(conMove_Horizontal, conMove_Vertical);

        //Fire 1 : 레이저 공격
        if (Input.GetButtonDown("Joystick1 Fire1") || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            //Debug.Log("Fire1");
            LaserAttack(Skill.Default);
        }        

        //Fire 2 : 에너지 공격
        if (Input.GetButtonDown("Joystick1 Fire2") || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            //Debug.Log("Fire2");
            EnergyAttack(Skill.Default);
        }

        //Fire 3 : 레이저 패턴 공격
        if(Input.GetButtonDown("Joystick1 Fire3") || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            //Debug.Log("Fire3");
            LaserAttack(Skill.Pattern);
        }
    }

    protected override void LaserAttack(Skill skill)
    {
        if (energy < consumeAmount)
        {
            //부족하면 경고 메세지
            WarnMessage<Player1_UI>();
            return;
        }

        base.LaserAttack(skill);
        //자원 소비
        UpdateEnergy<Player1_UI>(-consumeAmount);
        
    }

    protected override void EnergyAttack(Skill skill)
    {
        if(energy < consumeAmount)
        {
            //부족하면 경고 메세지
            WarnMessage<Player1_UI>();
            return;
        }

        base.EnergyAttack(skill);
        //자원 소비
        UpdateEnergy<Player1_UI>(-consumeAmount);
    }

    private void LateUpdate()
    {
        GenerateEnergy<Player1_UI>();
    }
}
