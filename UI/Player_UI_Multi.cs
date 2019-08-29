using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI_Multi : UI_Resource
{    
    public Vector3 enemyPos = new Vector3(-100.0f,-100.0f,-100.0f);
    public bool isEnemySet = false;

    [System.NonSerialized] public bool player1 = false;
    [System.NonSerialized] public bool player2 = false;
    bool isSet = false;

    // Start is called before the first frame update
    protected override void Awake()
    {
        Text[] textList = GetComponentsInChildren<Text>();

        foreach (Text text in textList)
        {
            switch (text.name)
            {
                case "Energy Available":
                    energyAvailable = text;
                    break;
                case "Message":
                    message = text;
                    break;
            }
        }
        //UpdateNumber(0, FindObjectOfType<PlayerController_Multi>().GetEnergy());

        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckNevi());
        if (isSet)
        {
            EnemyNevi();
        }      

    }

    void EnemyNevi()
    {
        if (isEnemySet)
        {
            navi.enabled = true;
            float enemy_posY = Mathf.Clamp(enemyPos.y, transform.position.y - (navi_posY_size / 2.0f), transform.position.y + (navi_posY_size / 2.0f));
            float posX = transform.localPosition.x;
            if (player1)
            {
                posX += navi_posX;
            }
            else if (player2)
            {
                posX -= navi_posX;
            }
            navi.transform.position = new Vector3(posX, enemy_posY, 0.0f);
        }
        else if (navi.enabled == true)
        {
            navi.enabled = false;
        }
    }

   IEnumerator CheckNevi()
    {
        while (!isSet)
        {
            Image[] imageList = GetComponentsInChildren<Image>();
            
            /*
            foreach (Image com in imageList)
            {
                Debug.Log(com.name);
            }
            */

            if (player1)
            {
                

                foreach (Image image in imageList)
                {
                    if(image.name == "Navi1")
                    {
                        navi = image;
                        navi.color = new Color(1, 1, 1, 1);
                        isSet = true;
                        break;
                    }
                }
            }
            else if (player2)
            {
                foreach (Image image in imageList)
                {
                    if (image.name == "Navi2")
                    {
                        navi = image;
                        navi.color = new Color(1, 1, 1, 1);
                        isSet = true;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    
}
