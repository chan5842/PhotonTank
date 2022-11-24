using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackAni : MonoBehaviour
{
    float scrollSpeed = 1f;
    MeshRenderer meshRenderer;
    TankMove tankMove;
    void Start()
    {
        tankMove = gameObject.transform.parent.GetComponent<TankMove>();
        meshRenderer = GetComponent<MeshRenderer>();    
    }

    void Update()
    {
        if(tankMove.isGetIn)
        {
            // GetAxis���� ���� ����
            var offset = Time.time * scrollSpeed * Input.GetAxisRaw("Vertical");
            // �ؽ����� �ϳ��� Diffuse �� ���
            meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            // ��ָ� ������ y�� ����
            meshRenderer.material.SetTextureOffset("_BumpMap", new Vector2(0f, offset));
        }
    }
}
