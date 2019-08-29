using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class ProjectileCollider_Multi : MonoBehaviourPunCallbacks
{
    [System.NonSerialized]public float lifeTime = 10.0f;

    float startTime;
    private void Start()
    {
        startTime = Time.fixedTime;
    }
    private void Update()
    {        
        if(Time.fixedTime > startTime + lifeTime && photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTrigwlgerEnter2D(Collider2D collision)
    {
        //Debug.Log(string.Format("owner : " + tag));
        //Debug.Log(string.Format(collision.tag));
        
        //발사의 주인과 다르면 발동
        if (!collision.tag.Equals(tag) && tag != "Untagged" && collision.tag != "Map")
        {
            //Debug.Log("Hit");
            collision.GetComponent<PlayerController_Multi>().Dead();            
        }
    }

    public void SetTag(string tagname)
    {     

        if (photonView.IsMine)
        {
            tag = tagname;
            photonView.RPC("Tagging", RpcTarget.Others, tag);
        }
        else
        {

        }
    }

    [PunRPC]
    void Tagging(string txt)
    {
        tag = txt;
    }
}
