using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheRide : MonoBehaviour
{
    GameObject player;
    ApacheCtrl apacheCtrl;
    FollowCamera followCam;
    readonly string playerTag = "Player";
    public bool isGrounded;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        apacheCtrl = GetComponentInParent<ApacheCtrl>();
        followCam = GameObject.Find("Main Camera").
           GetComponent<FollowCamera>();
    }

    void Update()
    {
        if(apacheCtrl.isGetIn && Input.GetKeyDown(KeyCode.E))
        {
            GetOutApache();
        }
    }

    void GetOutApache()
    {
        if (!isGrounded) return;    // 공중에 있으면 내릴수 없음

        followCam.target = player.transform;
        player.transform.GetChild(0).tag = "MainCamera";
        followCam.tag = "Untagged";
        followCam.GetComponent<AudioListener>().enabled = false;

        apacheCtrl.isGetOut = true;
        apacheCtrl.isGetIn = false;

        player.transform.position = this.transform.position + transform.right * Random.Range(7f, 15f);
        player.SetActive(true);
        player.GetComponentInChildren<AudioListener>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            apacheCtrl.isGetIn = true;
            apacheCtrl.isGetOut = false;

            player.GetComponentInChildren<AudioListener>().enabled = false;
            player.transform.GetChild(0).tag = "Untagged";
            other.gameObject.SetActive(false);

            followCam.tag = "MainCamera";
            followCam.GetComponent<AudioListener>().enabled = true;
            followCam.target = apacheCtrl.transform;
        }
        if(other.CompareTag("GROUND"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GROUND"))
        {
            isGrounded = false;
        }
    }
}
