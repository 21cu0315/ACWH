using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkill : MonoBehaviour
{
    [SerializeField] GameObject m_SPSkillEffect;
    // Start is called before the first frame update
    void PlaySE()
    {
        FindObjectOfType<SoundManager>().PlayAudioOneShot("SE_PlayerSP");
    }
    void PlaySE2()
    {
        FindObjectOfType<SoundManager>().PlayAudioOneShot("SE_SP2");
    }
    void PlaySEBoss()
    {
        FindObjectOfType<SoundManager>().PlayAudioOneShot("SE_BossSP");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateEffectBoss()
    {
        Time.timeScale = 1;
        FindObjectOfType<GameRuler>().m_player.m_SPSkillOn = false;
        Destroy(gameObject);
    }
    void CreateEffect() 
    {
        var tmp_pos = FindObjectOfType<Camera>().transform.position;
        var tmp_offsetPos = new Vector3(tmp_pos.x, tmp_pos.y, tmp_pos.z+10) ;
        Instantiate(m_SPSkillEffect, tmp_offsetPos, Quaternion.identity);
        Destroy(gameObject);
    }
}
