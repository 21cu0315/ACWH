using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    //�G�̎���
    [SerializeField] GameObject m_enemy;
    
    //�A�j���[�V����
    [SerializeField] Animator m_anim;

    //�G�𐶐����鏈��
   void SpwaneEnemy() 
    {
        //�G�𐶐�����
        Instantiate(m_enemy, transform.position, Quaternion.identity);

        //�X�|�[�������j��
        Destroy(this);
        Destroy(gameObject, 2f);
        
        //SE�𗬂�
        FindObjectOfType<SoundManager>().PlayAudioOneShot("Enemy_Spwan");
    }
    private void Start()
    {
        //�A�j���[�V�����𖳌�������
        m_anim.enabled = false;
    }

    //�G���X�|�[�����邽�߂̃R���W��������p
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[�Ɠ���������
        if (collision.gameObject.CompareTag("Player")) 
        {
            //�A�j���[�V������L��������
            m_anim.enabled = true;
            //�X�|�[�������j��
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
