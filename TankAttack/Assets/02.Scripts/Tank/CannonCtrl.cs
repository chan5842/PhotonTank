using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    Transform tr;
    public float rotSpeed = 300f;
    [SerializeField]
    float CurRotate = 0f;
    public float upperAngle = -30f;
    public float downAngle = 30f;
    PhotonView pv = null;

    Quaternion curRot = Quaternion.identity;

    TankMove tankMove;

    void Awake()
    {
        tankMove = gameObject.transform.parent.parent.GetComponent<TankMove>();
        tr = GetComponent<Transform>();

        pv = GetComponent<PhotonView>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else if(stream.isReading)
        {
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        //float angle = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;
        //tr.Rotate(angle, 0f, 0f);
        if(pv.isMine)
        {
            float wheel = -Input.GetAxis("Mouse ScrollWheel");
            float angle = Time.deltaTime * rotSpeed * wheel;

            if (wheel <= -0.01f)
            {
                CurRotate += angle;
                if (CurRotate > upperAngle)
                {
                    tr.Rotate(angle, 0f, 0f);
                }
                else
                    CurRotate = upperAngle;
            }
            else
            {
                CurRotate += angle;
                if (CurRotate < downAngle)
                {
                    tr.Rotate(angle, 0f, 0f);
                }
                else
                    CurRotate = downAngle;
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
