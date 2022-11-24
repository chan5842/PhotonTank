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
            // GetAxis보다 먼저 반응
            var offset = Time.time * scrollSpeed * Input.GetAxisRaw("Vertical");
            // 텍스쳐중 하나가 Diffuse 인 경우
            meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            // 노멀맵 오프셋 y값 변경
            meshRenderer.material.SetTextureOffset("_BumpMap", new Vector2(0f, offset));
        }
    }
}
