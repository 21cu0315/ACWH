/*
 * �쐬���ԁF2022/11/10
 * �쐬�ҁ@�F��
 * �������e�F�e�̔��ˏ���   �g�p�̃I�u�W�F�N�g�F"ProjectilesBase"���t���ăI�u�W�F�N�g
 * 
 * �X�V���e�F   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootProjectiles : MonoBehaviour
{
    //�e�̃I�u�W�F�N�g
    [SerializeField] private Transform m_tBullet;

    //���P�b�g
    public void Shoot(Transform _startPos, Transform _endPos, Enemy _target = null)
    {
        Vector3 tmp_pos = _startPos.position;
        tmp_pos.z = 0;
        //�e�쐬
        Transform tmp_bulletTransform = Instantiate(m_tBullet, tmp_pos, Quaternion.identity);

        ProjectilesBase tmp_projectiles = tmp_bulletTransform.GetComponent<ProjectilesBase>();

        if (tmp_projectiles == null)
        {
            Debug.LogError("Projectiles is NULL");
        }

        Vector3 tmp_shootDir = (_endPos.position - _startPos.position).normalized;
        //��������
        tmp_projectiles.SetUp(tmp_shootDir, _target);

        Debug.Log("Shoot");
    }

    //���[�U�[�e
    public IEnumerator Shoot(Transform _startPos, Vector3 _endPos, Enemy _target = null, float _delayTime = 0f)
    {
        //���b�̌�Ƀ��[�U�[�𐶐�
        yield return new WaitForSeconds(_delayTime);

        Transform tmp_bulletTransform = Instantiate(m_tBullet, _startPos.position, Quaternion.identity);

        ProjectilesBase tmp_projectiles = tmp_bulletTransform.GetComponent<ProjectilesBase>();

        if (tmp_projectiles == null)
        {
            Debug.LogError("Projectiles �͐ݒ肳��ĂȂ��ł�");
        }

        tmp_projectiles.SetUp(_startPos.position, _endPos, _target);

    }

    //���e
    public IEnumerator Shoot(Transform _startPos, Enemy _target = null, float _delayTime = 0f)
    {
        //���b�̌�ɔ��e�𐶐�
        yield return new WaitForSeconds(_delayTime);

        Transform tmp_bulletTransform = Instantiate(m_tBullet, _startPos.position, Quaternion.identity);
        ProjectilesBase tmp_projectiles = tmp_bulletTransform.GetComponent<ProjectilesBase>();

        //�������ɔ���
        Vector3 shootDir = new Vector3(0f, -1f, 0f);

        tmp_projectiles.SetUp(shootDir, _target);


    }

}
