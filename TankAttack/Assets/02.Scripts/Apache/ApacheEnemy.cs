using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheEnemy : MonoBehaviour
{
    Transform tr;
    public Transform path;
    public Transform[] pathTransform;
    public List<Transform> Nodes;
    public int curNode = 0;
    readonly string pointName = "PatrolPoint";

    public float rotDamping = 10f;
    public float moveSpeed = 3f;

    public bool isPatrol = true;    // 순찰하고 있는지
    public Transform TankTr;        // 탱크 트랜스폼 정보
    readonly string tankTag = "TANK";

    [Header("발사 관련 변수)")]
    public Transform[] firePos;
    public float CurrentDelay = 0f;
    public float MaxDelay = 80f;
    public GameObject Bullet;

    [SerializeField]
    AudioClip rotorSound;

    PhotonView pv = null;

    [Header("가까운 거리에 있는 플레이어 공격")]
    public List<GameObject> FoundObjects;
    public float shortDist;

    public GameObject[] playerTanks;
    public Transform target;

    void Start()
    {
        pv = PhotonView.Get(this);
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;

        tr = this.transform;
        //path = GameObject.Find(pointName).transform;
        //pathTransform = path.GetComponentsInChildren<Transform>();
        //Nodes = new List<Transform>();
        var point = GameObject.Find("PatrolPoint");
        if(point !=null)
        {
            point.GetComponentsInChildren<Transform>(Nodes);
            Nodes.RemoveAt(0);
        }
        TankTr = GameObject.FindGameObjectWithTag(tankTag).transform;
        rotorSound = Resources.Load("Sounds/RotorSound") as AudioClip;

        firePos[0] = tr.GetChild(0).GetChild(0).GetComponent<Transform>();
        firePos[1] = tr.GetChild(0).GetChild(1).GetComponent<Transform>();

        foreach (var tr in pathTransform)
        {
            if (tr != path.transform)
                Nodes.Add(tr);
        }
        //TankShortDist();

        SoundManager.soundManager.PlaySfx(tr.position, rotorSound, true);
    }

    #region 가장 가까운 탱크를 찾는 로직
    //void TankShortDist()
    //{
    //    FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(tankTag));
    //    // 가장 가까운 탱크와의 거리를 구함
    //    shortDist = Vector3.Distance(gameObject.transform.position, FoundObjects[0].transform.position);
    //    TankTr.position = FoundObjects[0].transform.position;
    //    foreach(GameObject found in FoundObjects)
    //    {
    //        float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);
    //        if(Distance < shortDist)
    //        {
    //            TankTr.position = found.transform.position;
    //            shortDist = Distance;
    //        }
    //        Debug.Log(TankTr.name);
    //    }
    //}
    #endregion


    void Update()
    {
        if(PhotonNetwork.connected)  // 유저가 접속중이면 실행
        {
            if (isPatrol)
            {
                PatrolMove();
                CheckWayPointDistance();
            }
            else
            {
                Attack();
            }
        } 
    }

    void PatrolMove()
    {
        Quaternion rot = Quaternion.LookRotation(Nodes[curNode].position - tr.position);

        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * rotDamping);
        tr.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        Search();
    }

    void CheckWayPointDistance()
    {
        // 현재 위치와 목적지 사이의 거리가 2.5이하라면
        if(Vector3.Distance(tr.position, Nodes[curNode].position) <= 2.5f)
        {
            // 마지막 노드면 처음 노드로 변경
            if (curNode == Nodes.Count - 1)
                curNode = 0;
            else
                curNode++;
        }
    }

    void Search()
    {
        playerTanks = GameObject.FindGameObjectsWithTag(tankTag);
        target = playerTanks[0].transform;
        // 탱크와 헬기 사이의 거리
        float dist = Vector3.Distance(tr.position, target.position);    
        //float dist = (TankTr.position - tr.position).magnitude;
        if(dist <= 75f)
        {
            isPatrol = false;
        }
    }
    void Attack()
    {
        playerTanks = GameObject.FindGameObjectsWithTag(tankTag);
        target = playerTanks[0].transform;
        float Dist = Vector3.Distance(tr.position, target.position);
        float dist2D;

        foreach(var tank in playerTanks)
        {
            // 자기 자신과 배열에 담은 탱크의 전체 거리의 크기
            dist2D = (tank.transform.position - tr.position).sqrMagnitude;
            if(dist2D < Dist)
            {
                target = tank.transform;
            }
        }
        Vector3 targetDist = target.position - tr.position;
        tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(targetDist),
            Time.deltaTime * rotDamping);

        if(targetDist.magnitude > 75f)
        {
            isPatrol = true;
        }
        CurrentDelay -= 0.1f;

        if(CurrentDelay <= 0f)
        {
            Fire();
            CurrentDelay = MaxDelay;
        }
    }

    void Fire()
    {
        Instantiate(Bullet, firePos[0].position, firePos[0].rotation);
        Instantiate(Bullet, firePos[1].position, firePos[1].rotation);
    }
}
