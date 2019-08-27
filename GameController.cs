using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public Tilemap availableArea;

    public float energyGenTime;
    public int genEnergyMount = 0;
    public int initEnergy = 0;

    List<Vector3Int> flagList = new List<Vector3Int>();

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

    public bool OnFlag(Vector3Int pos)
    {
        if(flagList.Exists(x => x == pos))
        {
            //Debug.Log("Cannot spawn here");
            return false;
        }
        else
        {
            //Debug.Log("Can spawn here");
            flagList.Add(pos);
            return true;
        }
    }

    public void OffFlag(Vector3Int pos)
    {
        flagList.Remove(pos);
    }
}
