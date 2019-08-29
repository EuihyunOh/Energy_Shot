using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_Button : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1280, 720, false);


    }
public void StartClick()
    {
        SceneManager.LoadScene(1);
    }

    public void MultiPlayeClick()
    {
        SceneManager.LoadScene(3);
    }

    public void SettingClick()
    {

    }

    public void QuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else        
        Application.Quit();
#endif
    }
}
