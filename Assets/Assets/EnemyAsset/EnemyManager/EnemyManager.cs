/*
 * �쐬���ԁF2022/12/07
 * �쐬�ҁ@�F��
 * 
 * �X�V���e�F2022/12/07�@20:00    �}�l�[�W���[�N���X�쐬�A���ׂĂ̓G���Ǘ�����}�l�[�W���[
*/

//  �V�[���̒��ŕK��EnemyManager���C���X�^���X���Ă��������B
//  �V�[���̒��ŕK��Player��Tag�� "Player" �ɂ��Ă��������B

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Enemy m_enemy;

    private void Start()
    {
        m_enemy = new Enemy();

        GameObject tmp_player = GameObject.FindGameObjectWithTag("Player");
        m_enemy.SetPlayer(tmp_player);
    }

    private void SpawnEnemy()
    {

    }

    private void DestoryEnemy()
    {

    }
}
