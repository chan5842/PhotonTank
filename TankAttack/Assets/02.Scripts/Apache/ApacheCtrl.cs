using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApacheCtrl : MonoBehaviour
{
    Transform tr;

    public float moveSpeed;
    public float rotSpeed;
    public float verticalSpeed;
    float powerDelta;
    float power;
    Vector3 moveVec;

    public bool isGetIn;
    public bool isGetOut = true;

    void Start()
    {
        tr = GetComponent<Transform>();
        moveSpeed = 0f;
        rotSpeed = 0f;
        power = 50f;
        verticalSpeed = 0f;
        powerDelta = 0f;
    }

    private void FixedUpdate()
    {
        if(isGetIn)
        {
            #region 헬기 좌우회전(A, D)
            if (Input.GetKey(KeyCode.A))
                rotSpeed += -0.2f;
            else if (Input.GetKey(KeyCode.D))
                rotSpeed += 0.2f;
            else
            {
                // 키를 누르지 않으면 자동으로 0으로 돌아옴
                if (rotSpeed > 0f) rotSpeed += -0.2f;
                else if (rotSpeed < 0f) rotSpeed += 0.2f;
                if (Mathf.Abs(rotSpeed) < 0.2f)
                    rotSpeed = 0f;
            }
            tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            #endregion

            #region 헬기 앞뒤 이동
            if (Input.GetKey(KeyCode.W))
                moveSpeed += 0.2f;
            else if (Input.GetKey(KeyCode.S))
                moveSpeed += -0.4f;
            tr.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            #endregion

            #region 브레이크
            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
            {
                if (moveSpeed > 0) moveSpeed = 0f;
                else if (moveSpeed < 0) moveSpeed = 0f;
                if (Mathf.Abs(moveSpeed) < 0.2f)
                    moveSpeed = 0f;

                if (Input.GetKey(KeyCode.Z)) verticalSpeed = 0f;
                else if (Input.GetKey(KeyCode.C)) verticalSpeed = 0f;
                if (Mathf.Abs(verticalSpeed) < 0.01f)
                    verticalSpeed = 0f;

            }
            #endregion

            #region 헬기 상하 이동
            if (Input.GetKey(KeyCode.Z))
                verticalSpeed += 0.04f;
            else if (Input.GetKey(KeyCode.C))
                verticalSpeed += -0.04f;
            else
            {
                if (Input.GetKey(KeyCode.Z)) verticalSpeed += -0.04f;
                else if (Input.GetKey(KeyCode.C)) verticalSpeed += 0.04f;
                verticalSpeed = 0f;
            }
            tr.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
            #endregion
        }
    }
}
