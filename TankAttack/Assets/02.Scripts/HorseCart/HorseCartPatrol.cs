using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCartPatrol : MonoBehaviour
{
    Transform tr;
    public Transform path;
    public Transform[] pathTransforms;
    public List<Transform> Nodes;
    public int curNode = 0;
    readonly string pointName = "PatrolPoints";

    public float rotDamping = 5f;
    public float moveDamping = 8f;

    public bool isPatrol = true;
    
    void Start()
    {
        tr = this.transform;
        path = GameObject.Find(pointName).transform;
        pathTransforms = path.GetComponentsInChildren<Transform>();
        Nodes = new List<Transform>();

        foreach(var tr in pathTransforms) // 포인트가 아닌 빈 오브젝트는 리스트에서 제거
        {
            if (tr != path.transform)
                Nodes.Add(tr);
        }
    }

    void Update()
    {
        if (isPatrol)
        {
            PatrolMove();
            CheckWayPointDistance();
        }
    }

    void PatrolMove()
    {
        Quaternion rot = Quaternion.LookRotation(Nodes[curNode].position - tr.position);
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * rotDamping);
        tr.Translate(Vector3.forward * Time.deltaTime * moveDamping);
    }
    void CheckWayPointDistance()
    {
        if(Vector3.Distance(tr.position, Nodes[curNode].position) <= 2.5f)
        {
            // 마지막 위치면 첫 위치로 목적지 변경
            if (curNode == Nodes.Count - 1)
                curNode = 0;
            else
                curNode++;
        }
    }
}
