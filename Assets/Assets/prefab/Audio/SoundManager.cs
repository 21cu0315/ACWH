using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private void Update()
    {
        //----------------------------------------debug-----------------------------------
        if (Input.GetKey(KeyCode.R))
        {
            FindObjectOfType<SoundManager>().PlaySELockOn();
            FindObjectOfType<SoundManager>().LoopAudio("SE_Title01");
        }

        if (Input.GetKey(KeyCode.P))
        {
            FindObjectOfType<SoundManager>().PlaySELockOn();
            FindObjectOfType<SoundManager>().StopAudio("SE_Title01");
        }
        //----------------------------------------debug-----------------------------------
    }
    // ���\�[�X�̃��X�g
    [SerializeField]
    private List<AudioClip> m_AudioClipSuper;

    //���T
    private Dictionary<string, AudioSource> m_AudioSourceDic;

    private void Awake()
    {
        m_AudioSourceDic = new Dictionary<string, AudioSource>();

        foreach (var item in m_AudioClipSuper)
        {
            //Clip�f�[�^��Dictionary�ɑ��
            AudioSource tmp_Audio = gameObject.AddComponent<AudioSource>();
            tmp_Audio.clip = item;

            //���T�ɒǉ�
            m_AudioSourceDic.Add(item.name, tmp_Audio);

            //Debug.LogWarning(item.name);
        }
    }

    //---------------------------------------------------------------
    //Play
    public void PlayAudio(string _key)
    {
        //key�ŌĂяo��
        if (m_AudioSourceDic.ContainsKey(_key))
        {
            m_AudioSourceDic[_key].Play();
        }
        else
        {
            Debug.LogError("Audio Key ���Ȃ��ł��B");
        }
    }
    public void PlayWhileNotPlaying(string _key)
    {
        //key�ŌĂяo��
        if (m_AudioSourceDic.ContainsKey(_key))
        {
            if (!m_AudioSourceDic[_key].isPlaying) 
            {
            m_AudioSourceDic[_key].Play();
            }
        }
        else
        {
            Debug.LogError("Audio Key ���Ȃ��ł��B");
        }
    }
    //OnPlay
    public void PlayAudioOneShot(string _key)
    {
        //key�ŌĂяo��
        if (m_AudioSourceDic.ContainsKey(_key))
        {
            m_AudioSourceDic[_key].PlayOneShot(m_AudioSourceDic[_key].clip);
        }
        else
        {
            Debug.LogError("Audio Key ���Ȃ��ł��B");
        }
    }

    //OnPlay
    public void PlayAudioOneShotWhileNotPlaying(string _key)
    {
        //key�ŌĂяo��
        if (m_AudioSourceDic.ContainsKey(_key))
        {
            if (!m_AudioSourceDic[_key].isPlaying)
            {
                m_AudioSourceDic[_key].PlayOneShot(m_AudioSourceDic[_key].clip);
            }
        }
        else
        {
            Debug.LogError("Audio Key ���Ȃ��ł��B");
        }
    }
    //Stop
    public void StopAudio(string _key)
    {
        //key�ŌĂяo��
        if (m_AudioSourceDic.ContainsKey(_key))
        {
            m_AudioSourceDic[_key].Stop();
        }
        else
        {
            Debug.LogError("Audio Key ���Ȃ��ł��B");
        }
    }

    //LoopPlay
    public void LoopAudio(string _key)
    {
        //key�ŌĂяo��
        if (m_AudioSourceDic.ContainsKey(_key))
        {
            m_AudioSourceDic[_key].loop = true;
            m_AudioSourceDic[_key].Play();
        }
        else
        {
            Debug.LogError("Audio Key ���Ȃ��ł��B");
        }
    }

    public void SetAudioVol(string _key,float _vol)
    {
        //key�ŌĂяo��
        if (m_AudioSourceDic.ContainsKey(_key))
        {
            m_AudioSourceDic[_key].volume = _vol;
        }
        else
        {
            Debug.LogError("Audio Key ���Ȃ��ł��B");
        }
    }

    public void PlayBgm()
    {
        string _key = "BGM";
        PlayAudio(_key);
    }

    public void PlaySEJump()
    {
        string _key = "";
        PlayAudioOneShot( _key );
    }

    public void PlaySESp()
    {
        string _key = "";
        PlayAudioOneShot(_key);
    }
    public void PlaySELockOn()
    {
        string _key = "SE_LockOn";
        PlayAudioOneShot(_key);
    }

    //---------------------------------------------------------------



    [SerializeField]
    AudioSource bgmAudioSource;

    [SerializeField]
    AudioSource seAudioSource;

    public float BgmVolume
    {
        get
        {
            return bgmAudioSource.volume;
        }
        set
        {
            bgmAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    public float SeVolume
    {
        get
        {
            return seAudioSource.volume;
        }
        set
        {
            seAudioSource.volume = Mathf.Clamp01(value);
        }
    }

    void Start()
    {
        GameObject soundManager = CheckOtherSoundManager();
        bool checkResult = soundManager != null && soundManager != gameObject;

        if (checkResult)
        {
            Debug.Log("�T�E���h�}�l�[�W���[���������݂��Ă���");
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);
    }

    GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    public void PlayBgm(AudioClip clip)
    {
        bgmAudioSource.clip = clip;

        if (clip == null)
        {
            return;
        }

        bgmAudioSource.Play();
    }

    public void PlaySe(AudioClip clip)
    {
        if (clip == null)
        {
            return;
        }

        seAudioSource.PlayOneShot(clip);
    }


    


}