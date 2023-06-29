using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPSkillObj : MonoBehaviour
{
    void SetSPSkillEnd() 
    {
        Time.timeScale = 1;
        FindObjectOfType<GameRuler>().m_player.m_SPSkillOn = false;
        Destroy(this.gameObject, 0.1f);
    }
   
}
