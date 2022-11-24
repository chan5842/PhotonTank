using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [HideInInspector]
    public string roomName = "";
    [HideInInspector]
    public int connectPlayer = 0;
    [HideInInspector]
    public int maxPlayer = 0;

    public Text textRoonName;
    public Text textConnectInfo;

    public void DisplayRoomData()
    {
        textRoonName.text = roomName;
        textConnectInfo.text = "(" + connectPlayer.ToString() + "/ " + maxPlayer.ToString() + ")";
    }
}
