/*
 * 作成時間：2022/12/07
 * 作成者　：董
 * 
 * 更新内容：2022/12/07　20:00    マネージャークラス作成、すべての敵を管理するマネージャー
*/

//  シーンの中で必ずEnemyManagerをインスタンスしてください。
//  シーンの中で必ずPlayerのTagを "Player" にしてください。

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
