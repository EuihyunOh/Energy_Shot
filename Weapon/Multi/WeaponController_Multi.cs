using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using Photon.Pun;
using Photon.Realtime;

public class WeaponController_Multi : MonoBehaviourPunCallbacks,IPunObservable
{
    public GameObject projectilePrefab;
    public float projectileLifeTime = 10.0f;
    public float speed = 5.0f;
    public float unitDuration = 1.0f;
    public int cost = 100;

    protected GameController gameController;
    protected Transform muzzle;
    protected Rigidbody2D projectileRb2D;
    protected GameObject projectile;
    protected Animator animator;
    

    protected float dir;
    protected bool isFire = false;
    protected bool isCharging = false;
    protected float fireStart = 0;


    protected void Awake()
    {
        tag = "Untagged";
        gameController = FindObjectOfType<GameController>();        
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        muzzle = GetComponentInChildren<Transform>();
        animator = GetComponent<Animator>();

        dir = transform.localScale.x > 0 ? 1.0f : -1.0f;
        
    }

    protected virtual void Update()
    {
        if (isFire)
        {
            UnitDurationCheck();
        }
    }

    //발사
    protected virtual void Fire()
    {        
        Vector3 trans = new Vector3(muzzle.position.x, muzzle.position.y, 0);
        projectile = PhotonNetwork.Instantiate(projectilePrefab.name, trans, Quaternion.identity);
        projectile.GetComponent<ProjectileCollider_Multi>().SetTag(tag);
        projectileRb2D = projectile.GetComponent<Rigidbody2D>();
        //Destroy(projectile, projectileLifeTime);
        //PhotonNetwork.Destroy(projectile);
        SetProjectileLifeTime(projectile);
    }

    protected virtual void UnitDurationCheck()
    {
        if (Time.fixedTime > fireStart + unitDuration)
        {
            animator.SetTrigger("Destroy");
        }
    }

    public void SetOwner(string ownerTag)
    {
        photonView.RPC("Tagging", RpcTarget.All, ownerTag);
    }

    protected void SetProjectileLifeTime(GameObject proj)
    {
        proj.GetComponent<ProjectileCollider_Multi>().lifeTime = projectileLifeTime;
    }

    protected void OnDestroy()
    {
        Tilemap map = gameController.availableArea;
        gameController.OffFlag(map.WorldToCell(transform.position));
    }

    //포톤뷰 파라미터
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    //RPC 통신
    [PunRPC]
    protected void Tagging(string txt)
    {
        tag = txt;
        //Debug.Log(txt);
    }
    

}

