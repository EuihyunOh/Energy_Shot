using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float dist = 0.0f;
    public float height =0.0f;


    public bool fixVertical = false;
    public bool fixHorizon = false;

    public float verticalMax = 0.0f;
    public float verticalMin = 0.0f;
    public float horizonMax = 0.0f;
    public float horizonMin = 0.0f;

    Vector3 pos;
    float x = 0.0f;
    float y = 0.0f;

    private void Start()
    {
        pos = transform.position;
    }

    private void LateUpdate()
    {
        if(target == null)
        {
            return;
        }


        if (!fixHorizon)
        {
            x = Mathf.Clamp(target.position.x + dist, horizonMin, horizonMax);            
        }
        if (!fixVertical)
        {
            y = Mathf.Clamp(target.position.y + height, verticalMin, verticalMax);
        }
        
        transform.position = new Vector3(x, y, -10.0f);
    }
}
