using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Resource : MonoBehaviour
{   
    public float updateTime = 0.4f;
    protected Text energyAvailable;
    protected Text energyConsume;
    //Color textColor;
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Text[] textList = GetComponentsInChildren<Text>(); 

        foreach(Text text in textList)
        {
            switch (text.name)
            {
                case "Energy Available": energyAvailable = text;
                    break;
                case "Energy Consume": energyConsume = text;
                    break;
            }
        }
        //extColor = energyConsume.color;

    }

    public void UpdateNumber(int fromNum, int toNum)
    {
        //Debug.Log("Energy Update");        
        StartCoroutine(NumberCount(fromNum, toNum));        
    }

    protected IEnumerator NumberCount(float startNum, float endNum)
    {
        //Debug.Log("Number Update Coroutine start!");
        float offset = Mathf.Abs(startNum - endNum) / updateTime;

        if (startNum < endNum)
        {
            while (startNum < endNum)
            {
                startNum += offset * Time.deltaTime;                
                energyAvailable.text = ((int)startNum).ToString();
                yield return null;
            }
        }
        else
        {
            while (startNum > endNum)
            {
                startNum -= offset * Time.deltaTime;
                energyAvailable.text = ((int)startNum).ToString();
                yield return null;
            }
        }

        energyAvailable.text = endNum.ToString();
    }


    /* 옆에 글씨로 알게 하는 방법
    void UpdateNumber()
    {
        switch (playerNum)
        {
            case 1:
                energyAvailable.text = FindObjectOfType<Player1Controller>().GetEnergy().ToString();
                break;
            case 2:
                energyAvailable.text = FindObjectOfType<Player2Controller>().GetEnergy().ToString();
                break;
        }
    }

    void ConsumeEffect(int n)
    {
        energyConsume.text = "-" + n.ToString();  
        energyConsume.enabled = true;
        Vector3 pos = energyConsume.transform.position;

               
    }

    IEnumerator ScoreEffect()
    {
        float time = 0.0f;

        Color fadeColor = textColor;
        fadeColor.a = Mathf.Lerp(1.0f, 0.0f, time);

        while(fadeColor.a > 0.0f)
        {
            time += Time.deltaTime / fadeTime;
            fadeColor.a = Mathf.Lerp(1.0f, 0.0f, time);
            energyConsume.color = fadeColor;

            yield return null;
        }
    }
    */
}
