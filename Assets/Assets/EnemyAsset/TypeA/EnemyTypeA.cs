/*
 * �쐬���ԁF2022/11/0
 * �쐬�ҁ@�F��
 * �������e�F�GA�̏���   �g��Script�F"EnemyShootProjectiles"
 * 
 * �X�V���e�F   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeA : Enemy
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        EnemyLookAt(GetNearbyPlayer());
    }

    [Header("[ -�h���N���X �ˌ����W - ]")]
    [SerializeField] protected Transform m_gunStartPos;
    [SerializeField] protected Transform m_gunEndPos;

    protected override void EnemyShoot()
    {
        if (m_gunStartPos != null && m_gunEndPos != null)
        {
            GetComponent<EnemyShootProjectiles>().Shoot(m_gunStartPos, m_gunEndPos);
        }
        else
        {
            Debug.LogError("firePos is null");
        }
    }

    //���S�A�j���V�����̃e�X�g�p
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            m_hp = 0;
        }
    }
}




