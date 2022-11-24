using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;
    AudioClip fireSound;
    float fireRate = 2f;  // 발사 대기 시간
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
            // 원격 네트워크 플레이어의 탱크에 RPC를 사용해 원격으로 Fire함수를 호출
            pv.RPC("Fire", PhotonTargets.Others, null);
        }
    }

    // 다른 네트워크 유저가 이 함수를 원격지에서 공유 할 수 있다.
    [PunRPC]
    void Fire()
    {
        nextFire = Time.time + fireRate; // 0.2초마다 발사
        Instantiate(bullet, firePos.position, firePos.rotation);
        SoundManager.soundManager.PlaySfx(transform.position, fireSound);
    }
}
