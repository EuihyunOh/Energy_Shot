using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro_Button : MonoBehaviour
{
    public void StartClick()
    {
        SceneManager.LoadScene(1);
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
