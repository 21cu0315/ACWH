using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] GameObject m_Boss;
    [SerializeField] Animator m_anim;
    void SpwaneBoss()
    {
        Instantiate(m_Boss);
        Destroy(this);
        Destroy(gameObject, 2f);
        FindObjectOfType<SoundManager>().PlayAudioOneShot("Enemy_Spwan");


    }
    private void Start()
    {
        m_anim.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_anim.enabled = true;
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
