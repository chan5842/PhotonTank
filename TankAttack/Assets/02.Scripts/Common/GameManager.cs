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
            PointList.RemoveAt(0);  // 첫번째 부모 오브젝트는 제거
        }


        // 로비 씬에서 넘어오면 다시 포톤 클라우드 서버에서 메시지를 수신
        PhotonNetwork.isMessageQueueRunning = true;
        CreateTank();
        GetConnectPlayerCount();

        // 0.2초 간격으로 호출, 3초마다 헬기 생성
        InvokeRepeating("CreateApache", 0.2f, 3f);
    }

    private void Start()
    {
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.NickName + "]Connected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);    // 자기 자신을 포함한 모두 메시지로
    }

    void CreateApache()
    {
        int count = (int)GameObject.FindGameObjectsWithTag("APACHE").Length;
        if(count < 10)
        {
            int idx = Random.Range(0, PointList.Count);
            // 씬에 고정적으로 생성하는 함수
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
        // 랜덤한 위치에 탱크 생성
        float pos = Random.Range(-100f, 100f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20f, pos), Quaternion.identity, 0);
    }

    void GetConnectPlayerCount()
    {
        Room curRoom = PhotonNetwork.room;
        txtConnet.text = "<color=#ff0000>"+ curRoom.PlayerCount.ToString() +  "</color>" + " / " + curRoom.MaxPlayers.ToString();
    }

    // 네트워크 플레이어가 접속 햇을 때 호출 함
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
        // 접속 종료시 빨간색으로 로그 표시
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.player.NickName + "]DisConnected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        // 현재 룸을 빠져나가며 생성한 모든 네트워크 객체를 삭제한다.
        PhotonNetwork.LeaveRoom();
    }
    // 롬에서 접속 종료 되었을떄 호출되는 콜백 함수
    void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}
