using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_UI : UI_Resource
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        UpdateNumber(0, FindObjectOfType<Player1Controller>().GetEnergy());
    }

    // Update is called once per frame
    void Update()
    {
        EnemyNevi();
    }

    void EnemyNevi()
    {
        if (FindObjectOfType<Player2Controller>() != null)
        {
            float enemy_posY = Mathf.Clamp(FindObjectOfType<Player2Controller>().transform.position.y, transform.position.y - (navi_posY_size / 2.0f), transform.position.y + (navi_posY_size / 2.0f));
            float posX = transform.position.x + navi_posX;
            navi.transform.position = new Vector3(posX, enemy_posY, 0.0f);
        }
        else
        {
            navi.enabled = false;
        }
    }
    
}
