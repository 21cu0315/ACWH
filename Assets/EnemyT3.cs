using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT3 : EnemyBase
{
    float m_distance = 5f;

    //移動するために、起点と終点のX座標の変数を事前に用意する
    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;

    //敵のY座標
    [SerializeField] private float m_Ypos;

    //移動方向
    [SerializeField] private bool ToRightSide = true;

    //タイマー
    private float timer = 5f;
    [SerializeField] private float floatTimer = 0f;

    //敵参照用
    Rigidbody2D m_rb;



    private void Start()
    {
        //起点と終点のX座標を初期化
        pointA = new Vector2(transform.position.x - m_distance, 0 + transform.position.y);
        pointB = new Vector2(transform.position.x + m_distance, 0 + transform.position.y);

        //Y座標を初期化
        m_Ypos = transform.position.y;
        
        //敵を参照する
        m_rb = GetComponent<Rigidbody2D>();

        //敵の位置を起点と終点の真ん中に用意する
        m_rb.position = Vector2.Lerp(pointA, pointB, 0.5f);
    }

    protected override void SubClass()
    {
        //タイマーを回す
        timer += Time.fixedDeltaTime;
        floatTimer += Time.fixedDeltaTime;

        //右に移動する時
        if (ToRightSide)
        {
            //プレイヤーを見つかった時
            if (m_playerFound)
            {
                //タイマーを早く回す
                timer += 0.05f*2f;
            }
            else 
            {
                //タイマーを回す
                timer += 0.05f;
            }
            //タイマーが切った時
            if (timer >= 10)
            {
                //左に移動する
                ToRightSide = false;
            }
            //向きを変える
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //左に移動する時
        else
        {
            //プレイヤーを見つかった時
            if (m_playerFound)
            {
                //タイマーを早く回す
                timer -= 0.05f * 2f;
            }
            else
            {
                //タイマーを回す
                timer -= 0.05f;
            }
            //タイマーが切った時
            if (timer <= 0)
            {
                //右に移動する
                ToRightSide = true;
            }
            //向きを変える
            transform.localScale = Vector3.one;
        }
        //移動処理
        m_rb.MovePosition( new Vector2(Mathf.Lerp(pointA.x, pointB.x, timer / 10f), transform.position.y-0.25f));
    }

    //コリジョンが発生した時
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        //プレイヤーと当たった時
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーを見つかった
            m_playerFound = true;
        }
    }

    //コリジョンが終わった時
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーを見つかりません
            m_playerFound = false;
        }
    }
}
