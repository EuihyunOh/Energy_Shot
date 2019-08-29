using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1.0";
    public string nickNameBase = "player";

    public GameObject messageComponent;

    RoomOptions options = new RoomOptions();
    TextMeshProUGUI textField;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        options.MaxPlayers = 2;
        options.IsVisible = true;
        options.PublishUserId = true;
        //룸 프러퍼티를 설정할 수 있다.
        //options.CustomRoomProperties = new Hashtable() { { "val", "0" } };

        textField = messageComponent.GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Login();
        textField.text = "Connecting to server...";
    }


    void Login()
    {
        //버전 정의로 같은 버전끼리만 매칭
        PhotonNetwork.GameVersion = gameVersion;
        //닉네임
        PhotonNetwork.NickName = nickNameBase;
        //포톤 온라인 연결
        PhotonNetwork.ConnectUsingSettings();

    }

    //룸 랜덤 접속
    void JoinRoom()
    {
        textField.text = "Find room...";
        PhotonNetwork.JoinRandomRoom();
    }

    //룸 생성
    void CreateRoom()
    {
        textField.text = "Create Room";
        PhotonNetwork.CreateRoom(null, options);
    }

    //서버에 연결
    public override void OnConnectedToMaster()
    {
        textField.text = "Connect to server!";
        //로비 접속
        PhotonNetwork.JoinLobby();
        Invoke("JoinRoom", 1.0f);
    }

    public override void OnJoinedRoom()
    {
        textField.text = "Waiting other player...";

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log(string.Format(player.UserId));
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        this.textField.text = "No Room Available";
        Invoke("CreateRoom", 1.0f);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        SceneManager.LoadScene(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            textField.text = "Match Making is Cancled";

            //네트워크 해제
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.Disconnect();
            Invoke("ReturnToMain", 1.0f);
        }
    }

    void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }
}
