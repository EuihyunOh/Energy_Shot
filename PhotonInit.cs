using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    /*
    public string gameVersion = "1.0";
    public string nickName = "Player";
    */

    public GameObject player1;
    public GameObject player1Spawn;
    public GameObject player2;
    public GameObject player2Spawn;


    GameController gameController;

    /*
    private void Awake()
    {
        //클라이언트 하나가 나머지에게 로드할 레벨을 정의
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    */
    // Start is called before the first frame update

    void Start()
    {        
      //  OnLogIn();

        gameController = GetComponent<GameController>();


        if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[0].UserId)
        {
            PhotonNetwork.Instantiate(player1.name, gameController.player1SpawnPoint.transform.position, Quaternion.identity);
            FindObjectOfType<Player_UI_Multi>().player1 = true;
            Debug.Log("player1 is set");
        }
        else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
        {
            PhotonNetwork.Instantiate(player2.name, gameController.player2SpawnPoint.transform.position, Quaternion.identity);
            FindObjectOfType<Player_UI_Multi>().player2 = true;
            Debug.Log("player2 is set");
        }

    }

    /*
    void OnLogIn()
    {
        //버전 정의로 같은 버전끼리만 연결
        PhotonNetwork.GameVersion = gameVersion;
        //클라이언트 닉네임
        PhotonNetwork.NickName = nickName;
        //포톤 온라인 연결
        PhotonNetwork.ConnectUsingSettings();
    }
        
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        //생성된 룸에 랜덤하게 접속
        PhotonNetwork.JoinRandomRoom();
    }

    //랜덤 접속 실패
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Fail to join");
        this.CreateRoom();
    }
        
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        //Instantiate(playerPrefab, gameController.player1SpawnPoint.transform.position, Quaternion.identity);
        if(PhotonNetwork.CountOfPlayersInRooms == 0)
        {
            PhotonNetwork.Instantiate(player1.name, gameController.player1SpawnPoint.transform.position, Quaternion.identity);
            Debug.Log("player1 is set");
        }
        if (PhotonNetwork.CountOfPlayersInRooms == 1)
        {
            PhotonNetwork.Instantiate(player2.name, gameController.player2SpawnPoint.transform.position, Quaternion.identity);
            Debug.Log("player2 is set");
        }

    }

    void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    */
}
