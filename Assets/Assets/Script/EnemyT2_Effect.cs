using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT2_Effect : MonoBehaviour
{
    [SerializeField] GameObject m_enemyT2Lazer; 
    void CreateLazer() 
    {
       Destroy(Instantiate(m_enemyT2Lazer, transform.position, Quaternion.identity),3f) ;
        FindObjectOfType<SoundManager>().PlayAudioOneShot("SE_PlayerShot");

    }
}
