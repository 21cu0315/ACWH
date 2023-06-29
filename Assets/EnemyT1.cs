using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT1 : EnemyBase
{
    //�G�̍s�����
    [SerializeReference] protected int m_state = 0;
    //�s����Ԃ̃J�E���^�[
    [SerializeReference] protected int m_stateCNT = 120;
    //����
    [SerializeField] GameObject m_weapon;
    //����̈ʒu
    [SerializeField] Transform m_weaponPos;

    protected override void SubClass() 
    {
       //�G�����ꂽ��
        if (m_SR.isVisible)
        {
            //�~�܂�p�J�E���^�[
            m_stopCNT = 90;
            //�s����Ԃ̃J�E���^�[����
            m_stateCNT--;
            //�J�E���^�[���[����菬�����ꍇ
            if (m_stateCNT <= 0)
            {
                //�s����Ԃ����Z�b�g
                m_state = 0;
                //�J�E���^�[�����Z�b�g
                m_stateCNT = 120;
            }
        }
        //�G������Ă��Ȃ���
        else
        {
            //�s����Ԃ����Z�b�g
            m_state = 0;
        }
        //�A�j���[�V���������Z�b�g
        m_anim.SetInteger("State", m_state);
    }

    //�R���W����������������
     protected override void  OnTriggerStay2D(Collider2D collision)
    {
        //�v���C���[�Ɠ���������
        if (collision.gameObject.CompareTag("Player"))
        {
            //��Ԃ�ς���
            m_state = 2;
            //�v���C���[��e������
            //���������߂�
            if (collision.gameObject.transform.position.x > gameObject.transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = Vector3.one;
            }
        }
    }
    //����𓊂���
    void ThrowWeapon()
    {
        //������쐬����
        GameObject tmp_weapon = Instantiate(m_weapon, m_weaponPos.position, Quaternion.identity);
        
        //SE�𗬂�
        FindObjectOfType<SoundManager>().PlayAudioOneShot("Enemy_T1Weapon");

        //��������������߂�
        if (transform.localScale.x < 0)
        {
            tmp_weapon.GetComponent<EnemyT1_Weapon>().m_dir = 1;

        }
        else
        {
            tmp_weapon.GetComponent<EnemyT1_Weapon>().m_dir = -1;

        }
    }
}
