using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Resource : MonoBehaviour
{   
    public float updateTime = 0.4f;
    public float messageDurationTime = 1.0f;
    public float navi_posX = 4.2f;
    public float navi_posY_size = 6.0f;
    protected Text energyAvailable;
    protected Text message;
    protected Image navi;
    
    protected bool isPrint = false;
    //Color textColor;
    

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        Text[] textList = GetComponentsInChildren<Text>(); 

        foreach(Text text in textList)
        {
            switch (text.name)
            {
                case "Energy Available": energyAvailable = text;
                    break;
                case "Message": message = text;
                    break;
            }
        }

        Image[] imageList = GetComponentsInChildren<Image>();

        foreach(Image image in imageList)
        {
            switch (image.name)
            {
                case "Navi": navi = image;
                    break;
            }
        }
        //extColor = energyConsume.color;

    }

    //자원 숫자 업데이트
    public void UpdateNumber(int fromNum, int toNum)
    {
        //Debug.Log("Energy Update");        
        StartCoroutine(NumberCount(fromNum, toNum));        
    }


    //메세지 출력
    public void UpdateMessage(string text)
    {
        if (!isPrint)
        {
            isPrint = true;
            StartCoroutine(Message(text));            
        }        
    }    

    //코루틴
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
  
    protected IEnumerator Message(string text)
    {
        message.enabled = true;
        message.text = text;        


        yield return new WaitForSeconds(messageDurationTime);
        message.enabled = false;
        isPrint = false;
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
