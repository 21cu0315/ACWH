using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    [Header("[ - 敵のスクリプト - ]")]
    public Enemy m_enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_enemy.EnemyOnTriggerEnter(collision);
        Debug.LogWarning("Is Player ");
    }

}
