/*
 * 作成時間：2022/11/10
 * 作成者　：董
 * 処理内容：弾の発射処理   使用のオブジェクト："ProjectilesBase"が付いてオブジェクト
 * 
 * 更新内容：   
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootProjectiles : MonoBehaviour
{
    //弾のオブジェクト
    [SerializeField] private Transform m_tBullet;

    //ロケット
    public void Shoot(Transform _startPos, Transform _endPos, Enemy _target = null)
    {
        Vector3 tmp_pos = _startPos.position;
        tmp_pos.z = 0;
        //弾作成
        Transform tmp_bulletTransform = Instantiate(m_tBullet, tmp_pos, Quaternion.identity);

        ProjectilesBase tmp_projectiles = tmp_bulletTransform.GetComponent<ProjectilesBase>();

        if (tmp_projectiles == null)
        {
            Debug.LogError("Projectiles is NULL");
        }

        Vector3 tmp_shootDir = (_endPos.position - _startPos.position).normalized;
        //初期方向
        tmp_projectiles.SetUp(tmp_shootDir, _target);

        Debug.Log("Shoot");
    }

    //レーザー弾
    public IEnumerator Shoot(Transform _startPos, Vector3 _endPos, Enemy _target = null, float _delayTime = 0f)
    {
        //何秒の後にレーザーを生成
        yield return new WaitForSeconds(_delayTime);

        Transform tmp_bulletTransform = Instantiate(m_tBullet, _startPos.position, Quaternion.identity);

        ProjectilesBase tmp_projectiles = tmp_bulletTransform.GetComponent<ProjectilesBase>();

        if (tmp_projectiles == null)
        {
            Debug.LogError("Projectiles は設定されてないです");
        }

        tmp_projectiles.SetUp(_startPos.position, _endPos, _target);

    }

    //爆弾
    public IEnumerator Shoot(Transform _startPos, Enemy _target = null, float _delayTime = 0f)
    {
        //何秒の後に爆弾を生成
        yield return new WaitForSeconds(_delayTime);

        Transform tmp_bulletTransform = Instantiate(m_tBullet, _startPos.position, Quaternion.identity);
        ProjectilesBase tmp_projectiles = tmp_bulletTransform.GetComponent<ProjectilesBase>();

        //下方向に爆発
        Vector3 shootDir = new Vector3(0f, -1f, 0f);

        tmp_projectiles.SetUp(shootDir, _target);


    }

}
