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

    [Header("��ũ ������")]
    public bool isGetIn;
    public bool isGetOut = true;

    Vector3 curPos = Vector3.zero;
    Quaternion curRot = Quaternion.identity;

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        // ��ũ�� ���� �����߽��� ���� ����
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
        pv = GetComponent<PhotonView>();
        // UDP�� ������ �˻縦 �ּ������� �Ѵ�
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        // Tank�� ������
        pv.ObservedComponents[0] = this;

        if(pv.isMine) // ���� �䰡 ��Ʈ��ũ �� ���� ���̶��
        {
            // ī�޶��� Ÿ���� �������� ����
            Camera.main.GetComponent<FollowCamera>().target = this.tr;
        }

        curPos = tr.position;
        curRot = tr.rotation;
    }

    // �������� �ۼ��� �ϴ� �Լ�
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)    // �۽�
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else if(stream.isReading)   // �ٸ� ��Ʈ��ũ �������� ����
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        //if(isGetIn)
        if(pv.isMine) // ���� �䰡 �� �Ŷ�� ����
        {
            Move();
        }
        else //  �ٸ� �������̶��
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
