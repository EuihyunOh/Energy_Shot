using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float energyGenTime;
    public int genEnergyMount = 0;
    public int initEnergy = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        string[] names = Input.GetJoystickNames();

        for (int i = 0; i < names.Length; i++)

        {

            Debug.Log("Connected Joysticks :: " + "Joystick" + (i + 1) + " = " + names[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
