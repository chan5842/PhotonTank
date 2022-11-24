using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UnityEngine.UI.Text txtConnet;
    public UnityEngine.UI.Text txtLogMsg;
    PhotonView pv = null;

    [SerializeField]
    List<Transform> PointList;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        var point = GameObject.Find("ApacheSpawnPoints");
        if(point != null)
        {
            point.GetComponentsInChildren<Transform>(PointList);
            PointList.RemoveAt(0);  // ù��° �θ� ������Ʈ�� ����
        }


        // �κ� ������ �Ѿ���� �ٽ� ���� Ŭ���� �������� �޽����� ����
        PhotonNetwork.isMessageQueueRunning = true;
        CreateTank();
        GetConnectPlayerCount();

        // 0.2�� �������� ȣ��, 3�ʸ��� ��� ����
        InvokeRepeating("CreateApache", 0.2f, 3f);
    }

    private void Start()
    {
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.NickName + "]Connected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);    // �ڱ� �ڽ��� ������ ��� �޽�����
    }

    void CreateApache()
    {
        int count = (int)GameObject.FindGameObjectsWithTag("APACHE").Length;
        if(count < 10)
        {
            int idx = Random.Range(0, PointList.Count);
            // ���� ���������� �����ϴ� �Լ�
            PhotonNetwork.InstantiateSceneObject("Apache",
                PointList[idx].position, PointList[idx].rotation, 0, null);
        }
    }

    [PunRPC]
    void LogMsg(string msg)
    {
        txtLogMsg.text = txtLogMsg.text + msg;
    }

    void CreateTank()
    {
        // ������ ��ġ�� ��ũ ����
        float pos = Random.Range(-100f, 100f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20f, pos), Quaternion.identity, 0);
    }

    void GetConnectPlayerCount()
    {
        Room curRoom = PhotonNetwork.room;
        txtConnet.text = "<color=#ff0000>"+ curRoom.PlayerCount.ToString() +  "</color>" + " / " + curRoom.MaxPlayers.ToString();
    }

    // ��Ʈ��ũ �÷��̾ ���� ���� �� ȣ�� ��
    void OnPhotonPlayerConnected(PhotonPlayer newplayer)
    {
        GetConnectPlayerCount();
    }
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {
        GetConnectPlayerCount();
    }
    public void OnClickExit()
    {
        // ���� ����� ���������� �α� ǥ��
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.player.NickName + "]DisConnected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        // ���� ���� ���������� ������ ��� ��Ʈ��ũ ��ü�� �����Ѵ�.
        PhotonNetwork.LeaveRoom();
    }
    // �ҿ��� ���� ���� �Ǿ����� ȣ��Ǵ� �ݹ� �Լ�
    void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}
