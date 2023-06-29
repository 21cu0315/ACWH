//----------------------------------------------------------
// ファイル名		：MagneticEnemy.cs
// 概要				：磁気タイプの敵の処理
// 作成者			：21CU0315 黄卓賢
// 更新内容			：2022/11/30 21CU0315 黄卓賢　作成
//----------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticEnemy : ProjectilesBase
{
    [SerializeField]private Collider2D metalCollider2D;
    [SerializeField] private Collider2D enemyCollider2D;

    private Vector2 metalOriginPos;

    private Vector2 enemyOriginPos;

    [SerializeField] private bool isActivated;

    private const float upSpeed = 0.01f;

    private const float downSpeed = 0.2f;

    [SerializeField] private float timer = 0.0f;

    private float timerCD = 5.0f;

    private float movementCnt = 0f;

    // Start is called before the first frame update
    void Start()
    {
        

        metalOriginPos = metalCollider2D.transform.position;

        enemyOriginPos.x = enemyCollider2D.transform.position.x;

        enemyOriginPos.y = enemyCollider2D.transform.position.y - 1.15f;

        isActivated = true;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (isActivated != true)
        {
            movementCnt += downSpeed;
            timer += Time.fixedDeltaTime;
            metalCollider2D.transform.position = Vector2.Lerp(enemyOriginPos, metalOriginPos, movementCnt);
            if (timer >= timerCD)
            {
                isActivated = true;

                timer = 0.0f;
                movementCnt = 0f;

                return;
            }
        }
        else if (isActivated == true)
        {
            movementCnt += upSpeed;
            timer += Time.fixedDeltaTime;
            metalCollider2D.transform.position = Vector2.Lerp(metalOriginPos, enemyOriginPos, movementCnt);
            if (timer >= timerCD)
            {
                isActivated = false;

                timer = 0.0f;
                movementCnt = 0f;

                return;
            }
        }

        
    }

}
