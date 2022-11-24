using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ride : MonoBehaviour
{
    GameObject player;
    TankMove tankMove;
    FollowCamera followCamera;
    readonly string playerTag = "Player";

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        tankMove = GetComponentInParent<TankMove>();
        followCamera = GameObject.Find("Main Camera").
            GetComponent<FollowCamera>();
    }

    private void Update()
    {
        // 탑승한 상태에서 E키를 누르면 하차
        //if (tankMove.isGetIn && Input.GetKeyDown(KeyCode.E))
        //{
        //    GetOutTank();
        //}
    }

    private void GetOutTank()
    {
        followCamera.target = player.transform;
        player.transform.GetChild(0).tag = "MainCamera";
        followCamera.tag = "Untagged";
        followCamera.GetComponent<AudioListener>().enabled = false;

        tankMove.isGetIn = false;
        tankMove.isGetOut = true;

        // 내리면 탱크 뒤쪽에서 내려짐
        player.transform.position = this.transform.position + transform.right * Random.Range(7f,15f);
        player.SetActive(true);
        player.GetComponentInChildren<AudioListener>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 닿으면 탑승
        if(other.CompareTag(playerTag))
        {
            tankMove.isGetIn = true;
            tankMove.isGetOut = false;
            player.GetComponentInChildren<AudioListener>().enabled = false;
            player.transform.GetChild(0).tag = "Untagged";
            other.gameObject.SetActive(false);

            followCamera.tag = "MainCamera";
            followCamera.GetComponent<AudioListener>().enabled = true;
            followCamera.target = tankMove.transform;
            
        }
    }
}
