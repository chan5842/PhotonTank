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
    void Awake()    // ��Ʈ��ũ�� ������ Start�� �ƴ� Awake���
    {
        // ���� Ŭ���� ����
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
        // �����÷��̾� �̸� ����
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);   // �̸� ���� ����

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;      // ���������� ����
        roomOptions.IsVisible = true;   // �� ��� ����Ʈ�� Ȱ��ȭ
        roomOptions.MaxPlayers = 20;    // �ִ� �ο���

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);    // �� ����
    }

    void OnPhotonCreateRoomFailed(object[] error)
    {
        Debug.Log(error[0].ToString()); // �����ڵ�
        Debug.Log(error[1].ToString()); // ������ ���� 
    }

    void OnReceivedRoomListUpdate() // ���� �����Ǿ��ų� �� ����Ʈ�� ���� �Ǿ��� �� �ݹ� �Լ�
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RoomItem"))
        {
            Destroy(obj);   // �� ��� �ֽ�ȭ
        }

        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            print(_room.Name);
            GameObject room = (GameObject)Instantiate(roomitem);
            room.transform.SetParent(ScrollContents.transform, false);  // ���� ��ǥ

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayer = _room.MaxPlayers;
            roomData.DisplayRoomData();
            // RoomItem�� Button������Ʈ�� Ŭ�� �̺�Ʈ�� �����Ҵ�
            roomData.GetComponent<UnityEngine.UI.Button>().
                onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });
        }
    }

    void OnJoinedLobby()    // �κ� �����ϸ� �ڵ����� ȣ��
    {
        Debug.Log("�κ� ����!");
        //PhotonNetwork.JoinRandomRoom(); // �������� �� ����
        userId.text = GetUserID();
    }

    string GetUserID()
    {
        // Ű ���� ����
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
        PhotonNetwork.CreateRoom("Rogame's Room"); // �� ����
        //PhotonNetwork.CreateRoom("YourRoom", RoomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    { 
        Debug.Log("�뿡 ����!");
        StartCoroutine(LoadBattleField());
        //CreateTank();
    }

    IEnumerator LoadBattleField()
    {
        // �� ��ȯ�߿��� ����Ŭ���� ������ ���� ��Ʈ��ũ �޽��� ���� �ߴ�
        PhotonNetwork.isMessageQueueRunning = false;
        // ��׶��� �� �ε�(�񵿱�)
        AsyncOperation ao = SceneManager.LoadSceneAsync("scBattleField");
        yield return ao;
    }

    public void OnClickJoinRandomRoom()
    {
        // ���� �÷��̾� �̸� ����
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);
        // �������� ����� ������ ����
        PhotonNetwork.JoinRandomRoom();
    }

    void OnClickRoomItem(string roomName)  // ���ϴ� ���� Ŭ������ �� ȣ��
    {
        PhotonNetwork.player.NickName = userId.text;
        PlayerPrefs.SetString("User_ID", userId.text);
        PhotonNetwork.JoinRoom(roomName);   // �ش� �濡 ����
    }

    private void OnGUI() // ������ ������ ��ũ���� �����ִ� �ݹ� �Լ�
    {
        // ȭ�鿡 ���� ��Ʈ��ũ ���� ������ ������
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }   
}
