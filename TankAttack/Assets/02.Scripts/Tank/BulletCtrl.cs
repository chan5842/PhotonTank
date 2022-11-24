using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    Rigidbody rb;
    public float Speed = 1500f;
    Transform tr;
    CapsuleCollider capsuleCollider;
    [SerializeField]
    GameObject ExplosionEffect;
    AudioClip expSfx;

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb.AddForce(tr.forward * Speed, ForceMode.Impulse);
        expSfx = Resources.Load("Sounds/missile_explosion") as AudioClip;
        ExplosionEffect = Resources.Load("Effects/SmallExplosionEffect") as GameObject;
        StartCoroutine(ExplosionCannon(3f));    // 3초 후 폭파한 이후에 제거
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(ExplosionCannon(0f));
    }


    IEnumerator ExplosionCannon(float time)
    {
        yield return new WaitForSeconds(time);
        capsuleCollider.enabled = false;
        rb.isKinematic = true;
        GameObject explosion = Instantiate(ExplosionEffect, tr.position, Quaternion.identity);
        Destroy(explosion,1f);
        SoundManager.soundManager.PlaySfx(tr.position, expSfx);
        GetComponent<TrailRenderer>().Clear();
        Destroy(gameObject, 1f); // trailRenderer가 있으므로 1초 후 제거
    }


}
