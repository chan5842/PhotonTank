using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviour
{
    Transform tr;
    RaycastHit hit;
    public float rotSpeed = 5f;     // �ͷ� ȸ�� �ӵ�
    TankMove tankMove;

    PhotonView pv = null;
    Quaternion curRot = Quaternion.identity;

    void Awake()
    {
        tankMove = GetComponentInParent<TankMove>();
        tr = GetComponent<Transform>();

        pv = GetComponent<PhotonView>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;
        curRot = tr.localRotation; // ��ũ �����Ӱ� ������
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else
        {
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        // ī�޶󿡼� ���콺�� ���� �߻�
        if (pv.isMine)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 6))
            {
                // ��ũ�� ��ǥ�� ���� ��ǥ�� ��ȯ
                Vector3 relative = tr.InverseTransformPoint(hit.point);
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;

                tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f);
            }
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, curRot, Time.deltaTime * 3f);
        }
        
        //if (tankMove.isGetIn)
        //{
           
        //}        
    }
}
