using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2_UI : UI_Resource
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        UpdateNumber(0, FindObjectOfType<Player2Controller>().GetEnergy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
