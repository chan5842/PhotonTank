using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour
{
    Transform tr;
    Rigidbody rb;
    PhotonView pv = null;

    public float moveSpeed = 12f;
    public float turnSpeed = 100f;

    float h = 0;
    float v = 0;

    [Header("탱크 승하차")]
    public bool isGetIn;
    public bool isGetOut = true;

    Vector3 curPos = Vector3.zero;
    Quaternion curRot = Quaternion.identity;

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        // 탱크나 차는 무게중심을 낮게 잡음
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
        pv = GetComponent<PhotonView>();
        // UDP는 데이터 검사를 최소한으로 한다
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        // Tank의 움직임
        pv.ObservedComponents[0] = this;

        if(pv.isMine) // 포톤 뷰가 네트워크 상 본인 것이라면
        {
            // 카메라의 타겟을 본인으로 변경
            Camera.main.GetComponent<FollowCamera>().target = this.tr;
        }

        curPos = tr.position;
        curRot = tr.rotation;
    }

    // 움직임을 송수신 하는 함수
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)    // 송신
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else if(stream.isReading)   // 다른 네트워크 움직임을 수신
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        //if(isGetIn)
        if(pv.isMine) // 포톤 뷰가 내 거라면 조종
        {
            Move();
        }
        else //  다른 유저것이라면
        {
            tr.position = Vector3.Lerp(tr.position, curPos, Time.deltaTime * 3f);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.deltaTime * 3f);
        }
        

    }

    void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.up * h * Time.deltaTime * turnSpeed);
    }
}
