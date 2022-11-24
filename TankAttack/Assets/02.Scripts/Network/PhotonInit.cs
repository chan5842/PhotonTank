using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviour
{
    public string version = "v1.0";
    public InputField userId;
    public InputField roomName;

    public GameObject ScrollContents;
    public GameObject roomitem;
    void Awake()    // 네트워크는 무조건 Start가 아닌 Awake사용
    {
        // 포톤 클라우드 접속
        if(!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings(version);
        roomName.text = "Room " + Random.Range(0, 999).ToString("000");
    }
    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;
        if (string.IsNullOrEmpty(roomName.text))
        {
            _roomName = "Room " + Random.Range(0, 999).ToString("000");
        }
        // 로컬플레이어 이름 설정
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);   // 이름 정보 저장

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;      // 공개방으로 설정
        roomOptions.IsVisible = true;   // 방 목록 리스트에 활성화
        roomOptions.MaxPlayers = 20;    // 최대 인원수

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);    // 방 생성
    }

    void OnPhotonCreateRoomFailed(object[] error)
    {
        Debug.Log(error[0].ToString()); // 오류코드
        Debug.Log(error[1].ToString()); // 오류난 원인 
    }

    void OnReceivedRoomListUpdate() // 방이 생성되었거나 룸 리스트가 변경 되었을 때 콜백 함수
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RoomItem"))
        {
            Destroy(obj);   // 방 목록 최신화
        }

        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            print(_room.Name);
            GameObject room = (GameObject)Instantiate(roomitem);
            room.transform.SetParent(ScrollContents.transform, false);  // 로컬 좌표

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayer = _room.MaxPlayers;
            roomData.DisplayRoomData();
            // RoomItem의 Button컴포넌트에 클릭 이벤트를 동적할당
            roomData.GetComponent<UnityEngine.UI.Button>().
                onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });
        }
    }

    void OnJoinedLobby()    // 로비에 입장하면 자동으로 호출
    {
        Debug.Log("로비에 입장!");
        //PhotonNetwork.JoinRandomRoom(); // 무작위의 방 접속
        userId.text = GetUserID();
    }

    string GetUserID()
    {
        // 키 값을 예약
        string userId = PlayerPrefs.GetString("User_ID");
        if(string.IsNullOrEmpty(userId))
        {
            userId = "User_" + Random.Range(0, 999).ToString("000");
        }
        return userId;
    }

    void OnPhotonRandomJoinFailed()
    {
        print("No Room!. ");
        PhotonNetwork.CreateRoom("Rogame's Room"); // 방 생성
        //PhotonNetwork.CreateRoom("YourRoom", RoomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    { 
        Debug.Log("룸에 입장!");
        StartCoroutine(LoadBattleField());
        //CreateTank();
    }

    IEnumerator LoadBattleField()
    {
        // 씬 전환중에는 포톤클라우드 서버로 부터 네트워크 메시지 수신 중단
        PhotonNetwork.isMessageQueueRunning = false;
        // 백그라운드 씬 로딩(비동기)
        AsyncOperation ao = SceneManager.LoadSceneAsync("scBattleField");
        yield return ao;
    }

    public void OnClickJoinRandomRoom()
    {
        // 로컬 플레이어 이름 설정
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);
        // 무작위로 추출된 방으로 입장
        PhotonNetwork.JoinRandomRoom();
    }

    void OnClickRoomItem(string roomName)  // 원하는 방을 클릭했을 때 호출
    {
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);
        PhotonNetwork.JoinRoom(roomName);   // 해당 방에 입장
    }

    private void OnGUI() // 글자의 형식을 스크린에 보여주는 콜백 함수
    {
        // 화면에 포톤 네트워크 접속 정보를 보여줌
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }   
}
