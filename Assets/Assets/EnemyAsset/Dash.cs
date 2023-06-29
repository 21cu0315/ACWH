using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Enemy
{
    [SerializeField] private Transform playerPos;

    [SerializeField] private float timer;

    [SerializeField] private bool playerInRange = false;

    private bool playerOnRightSide = false;

    [SerializeField] private bool startedAttack = false;

    private float enemyMoveX;

    private float playerInitialX;

    private bool dashRight;

    private const float detectionRange = 7.0f;

    enum AttackPattern
    {
        ready = 0,
        chargeTP,
        teleport,
        charge,
        dash,
    };

    [SerializeField] private AttackPattern enemyAttack;

    // Start is called before the first frame update
    protected override void Start()
    {
        enemyAttack = 0;
        enemyMoveX = m_col.transform.position.x;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (playerPos.transform.position.x < m_col.transform.position.x)
        {
            m_col.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
           
        }
        else
        {
            m_col.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        }

        if (startedAttack != true)
        {
            detectPlayerPos();
        }
        timer += Time.deltaTime;
        if (startedAttack != true)
        {
            if (playerInRange == true)
            {
                enemyAttack = AttackPattern.chargeTP;
            }
            else
            {
                enemyAttack = AttackPattern.ready;
            }
        }
        
        switch (enemyAttack)
        {
            case AttackPattern.ready:
                enemyMoveX = m_col.transform.position.x;
                ResetTimer();
                break;
            case AttackPattern.chargeTP:
                playerInitialX = playerPos.transform.position.x;
                if (timer >= 2.5f)
                {
                    if (playerInRange == true)
                    {
                        enemyAttack = AttackPattern.teleport;
                        startedAttack = true;
                        ResetTimer();
                    }
                    else
                    {
                        enemyAttack = AttackPattern.ready;
                    }
                   
                }
                break;
            case AttackPattern.teleport:
                if (playerOnRightSide == false)
                {
                    enemyMoveX = playerInitialX - 2.0f;
                }
                else
                {
                    enemyMoveX = playerInitialX + 2.0f;
                }
                enemyAttack = AttackPattern.charge;
                ResetTimer();
                break;
            case AttackPattern.charge:
                if (timer >= 1.0f) 
                {
                    enemyAttack = AttackPattern.dash;
                    ResetTimer();
                }
                else
                {
                    if (playerOnRightSide == false)
                    {
                        enemyMoveX -= 0.005f;
                        dashRight = true;
                    }
                    else
                    {
                        enemyMoveX += 0.005f;
                        dashRight = false;
                    }
                }
                break;
            case AttackPattern.dash:

                if (dashRight == false)
                {
                    enemyMoveX -= 0.25f;
                }
                else
                {
                    enemyMoveX += 0.25f;
                }

                if (timer >= 0.5f)
                {
                    enemyAttack = AttackPattern.ready;
                    ResetTimer();
                    Reset();
                }

                break;
        }
        if (enemyAttack != AttackPattern.ready) 
        {
            m_col.transform.position = new(enemyMoveX, -1.8f);
        }
    }

    

    private void detectPlayerPos()
    {
        if (playerPos.transform.position.x < m_col.transform.position.x)
        {
            playerOnRightSide = false;
            if (m_col.transform.position.x - playerPos.transform.position.x < detectionRange)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }
        }
        else
        {
            playerOnRightSide = true;
            if (playerPos.transform.position.x- m_col.transform.position.x < 5.0f)
            {
                playerInRange = true;
            }
            else
            {
                playerInRange = false;
            }
        }
    }

    private void Reset()
    {
        playerInRange = false;

        playerOnRightSide = false;

        dashRight = false;

        startedAttack = false;

        enemyMoveX = m_col.transform.position.x;

    }

    private void ResetTimer()
    {
        timer = 0.0f;
    }

    protected override void EnemyShoot()
    {
        
    }
}
