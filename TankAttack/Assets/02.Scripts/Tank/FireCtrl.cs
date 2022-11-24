using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    AudioClip fireSound;
    float fireRate = 2f;  // �߻� ��� �ð�
    float nextFire = 0f;

    TankMove tankMove;
    PhotonView pv = null;
    void Awake()
    {
        //bullet = Resources.Load("Bullet") as GameObject;
        fireSound = Resources.Load("Sounds/CannonFire") as AudioClip;
        tankMove = GetComponent<TankMove>();

        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (MouseHover.mouseHover.isHover) return;

        if(Input.GetMouseButtonDown(0) && Time.time > nextFire && pv.isMine)
        {
            Fire();
            // ���� ��Ʈ��ũ �÷��̾��� ��ũ�� RPC�� ����� �������� Fire�Լ��� ȣ��
            pv.RPC("Fire", PhotonTargets.Others, null);
        }
    }

    // �ٸ� ��Ʈ��ũ ������ �� �Լ��� ���������� ���� �� �� �ִ�.
    [PunRPC]
    void Fire()
    {
        nextFire = Time.time + fireRate; // 0.2�ʸ��� �߻�
        Instantiate(bullet, firePos.position, firePos.rotation);
        SoundManager.soundManager.PlaySfx(transform.position, fireSound);
    }
}
