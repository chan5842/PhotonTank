using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float height = 5f;
    public float distance = 7f;
    public float targetOffset = 2f;
    public float moveDamping = 15f;
    public float rotDamping = 20f;

    Transform tr;
    [SerializeField]
    AudioClip bgm;

    void Start()
    {
        tr = GetComponent<Transform>();
        bgm = Resources.Load("Sounds/BGM") as AudioClip;
        SoundManager.soundManager.PlaySfx(tr.position, bgm, true);
    }
    
    void LateUpdate()
    {
        if (target == null)
            return;
        Vector3 camPos = target.position - (target.forward * distance)
                                     + (target.up * height);

        transform.position = Vector3.Lerp(transform.position, camPos, Time.deltaTime * moveDamping);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * rotDamping);
        transform.LookAt(target.position + (target.up * targetOffset));
    }

    private void OnDrawGizmos()
    {
        if (target == null)
            return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
        Gizmos.DrawWireSphere(transform.position, 1f);
        
    }
}
