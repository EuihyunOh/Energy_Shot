using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(string.Format("owner : " + tag));
        //Debug.Log(string.Format(collision.tag));
        //Debug.Log("Hit");
        //발사의 주인과 다르면 발동
        if (!collision.tag.Equals(tag) && tag != "Untagged")
        {            
            switch (collision.tag)
            {
                case "Player1":
                    collision.GetComponent<Player1Controller>().Dead();

                    break;

                case "Player2":
                    collision.GetComponent<Player2Controller>().Dead();
                    break;
            }
            
        }
    }
}
