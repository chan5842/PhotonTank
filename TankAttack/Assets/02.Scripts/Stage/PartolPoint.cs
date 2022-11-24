using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartolPoint : MonoBehaviour
{
    public Color lineColor;
    public List<Transform> Nodes;
    float _radius = 3f;

    void OnDrawGizmos()
    {
        // 자식인 Point의 트랜스폼 정보를 가져옴
        Transform[] transforms = GetComponentsInChildren<Transform>();
        Nodes = new List<Transform>();

        //for(int i=0; i<transforms.Length; i++)
        foreach (var t in transforms)
        {
            // 부모인 자기 자신을 제외한 Point를 리스트에 담는다
            if(t != this.transform)
                Nodes.Add(t);
        }
        for (int i = 0; i < Nodes.Count; i++)
        {
            Vector3 curNode = Nodes[i].position;    // 현재 노드
            Vector3 prevNode = Vector3.zero;        // 이전 노드

            if (i > 0)
                prevNode = Nodes[i - 1].position;
            else if (i == 0 && Nodes.Count > 1)
                prevNode = Nodes[Nodes.Count - 1].position;

            Gizmos.DrawSphere(curNode, _radius);
            //Gizmos.color = lineColor;
            Gizmos.DrawLine(prevNode, curNode);
        }

    }

    

}
