using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float sfxVolume = 1f;
    public bool isSfxMute = false;
    public static SoundManager soundManager;

    void Awake()
    {
        if (soundManager == null)
            soundManager = this;
        else if (soundManager != this)
            Destroy(this);

        DontDestroyOnLoad(this);
    }

    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        if (isSfxMute) return;

        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = sfx;
        source.minDistance = 10f;
        source.maxDistance = 80f;
        source.volume = sfxVolume;
        source.Play();

        Destroy(soundObj, sfx.length);  
    }
    public void PlaySfx(Vector3 pos, AudioClip sfx, bool loop)
    {
        if (isSfxMute) return;

        GameObject soundObj = new GameObject("BGM");
        soundObj.transform.position = pos;
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = sfx;
        source.minDistance = 10f;
        source.maxDistance = 80f;
        source.volume = sfxVolume;
        source.loop = loop;
        source.Play();
    }
}
