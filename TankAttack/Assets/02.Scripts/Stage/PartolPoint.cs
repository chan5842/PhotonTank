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
        // �ڽ��� Point�� Ʈ������ ������ ������
        Transform[] transforms = GetComponentsInChildren<Transform>();
        Nodes = new List<Transform>();

        //for(int i=0; i<transforms.Length; i++)
        foreach (var t in transforms)
        {
            // �θ��� �ڱ� �ڽ��� ������ Point�� ����Ʈ�� ��´�
            if(t != this.transform)
                Nodes.Add(t);
        }
        for (int i = 0; i < Nodes.Count; i++)
        {
            Vector3 curNode = Nodes[i].position;    // ���� ���
            Vector3 prevNode = Vector3.zero;        // ���� ���

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
