using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankDamage : MonoBehaviour
{
    MeshRenderer[] meshRenderer;    // 피격후 사망하면 메시렌더러 비활성화
    BoxCollider[] colliders;
    [SerializeField]
    GameObject expEffect;           // 탱크 폭파 이펙트
    TankMove tankMove;
    FireCtrl fireCtrl;
    Rigidbody rb;

    readonly string bulletTag = "T_Bullet";

    int initHp = 100;
    [SerializeField]
    int curHp = 0;

    [Header("UI")]
    public Image HpBar;
    public Canvas canvas;

    void Start()
    {
        meshRenderer = GetComponentsInChildren<MeshRenderer>();
        colliders = GetComponentsInChildren<BoxCollider>();
        expEffect = Resources.Load("Effects/SmallExplosionEffect") as GameObject;
        tankMove = GetComponent<TankMove>();
        fireCtrl = GetComponent<FireCtrl>();
        rb = GetComponent<Rigidbody>();
        curHp = initHp;

        HpBar.color = Color.green;
        HpBar.fillAmount = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(curHp >0 && other.CompareTag(bulletTag))
        {
            curHp -= 34;
            curHp = Mathf.Clamp(curHp, 0, 100);
            HPUIManager();
            if (curHp <= 0)
            {
                StartCoroutine(ExplosionTank());

            }
        }
    }

    private void HPUIManager()
    {
        HpBar.fillAmount = (float)curHp / (float)initHp;
        if (HpBar.fillAmount <= 0.3f)
            HpBar.color = Color.red;
        else if (HpBar.fillAmount <= 0.5f)
            HpBar.color = Color.yellow;
    }

    IEnumerator ExplosionTank()
    {
        Object effect = GameObject.Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        SetTankVisible(false);
        // 박싱 : 사용하는데 단점은 없지만 많이 사용하면 문제가 발생하니 주의
       
        // 5초후 리스폰
        yield return new WaitForSeconds(5f);
        curHp = initHp;
        HpBar.color = Color.green;

        SetTankVisible(true);
    }

    private void SetTankVisible(bool isVisible)
    {
        foreach (var _renderer in meshRenderer)
        {
            _renderer.enabled = isVisible;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = isVisible;
        }
        tankMove.enabled = isVisible;
        fireCtrl.enabled = isVisible;
        rb.isKinematic = !isVisible;
        canvas.enabled = isVisible;
        HpBar.fillAmount = (float)curHp / initHp;
        
    }
}
