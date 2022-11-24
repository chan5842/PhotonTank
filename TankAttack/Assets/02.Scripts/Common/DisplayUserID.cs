using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUserID : MonoBehaviour
{
    public Text userId;
    PhotonView pv = null;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        userId.text = pv.owner.NickName;    // �̸� ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
