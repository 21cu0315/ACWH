using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : ProjectilesBase
{
    [SerializeField] private Transform electricPos;
    [SerializeField] private Transform enemyPos;
    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform leftDetectPoint;
    [SerializeField] private Transform rightDetectPoint;

    private float electricPosOriginX;
    private float electricPosOriginY;

     private bool startElectric = false;

    [SerializeField] private float timer;

    private bool outOfRange = false;

    private bool playerInRange = false;

    private float moveX = 0.0f;

    private float moveY = 0.0f;

    private bool shootRight = false;

    // Start is called before the first frame update
    void Start()
    {
        electricPosOriginX = electricPos.position.x;
        electricPosOriginY = electricPos.position.y;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (playerPos.position.x >= leftDetectPoint.position.x && playerPos.position.x <= rightDetectPoint.position.x) 
        {
            playerInRange = true;
            if (playerPos.position.x < enemyPos.transform.position.x)
            {
                enemyPos.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                if (startElectric == false)
                {
                    shootRight = false;
                }
            }
            else if (playerPos.position.x > enemyPos.transform.position.x)
            {
                enemyPos.transform.localRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                if (startElectric == false)
                {
                    shootRight = true;
                }
            }
            
        }
        else
        {
            playerInRange = false;
        }

        if (startElectric == false)
        {
            if (shootRight == true)
            {
                moveX = electricPosOriginX + 1.0f;
            }
            else
            {
                moveX = electricPosOriginX - 1.0f;
            }
            moveY = electricPosOriginY;
            if (playerInRange == true) 
            {
                timer += Time.deltaTime;
                if (timer >= 3.0f)
                {
                    startElectric = true;
                    Reset();
                }
            }
        }
        else if (startElectric == true)
        {
            moveY = enemyPos.transform.position.y - 1.0f;
            if (outOfRange == false)
            {
                outOfRange = true;
            }
            else
            {
                if (shootRight == true)
                {
                    moveX += 0.1f;
                }
                else
                {
                    moveX -= 0.1f;
                }
                timer += Time.deltaTime;
                if (timer >= 2.5f)
                {
                    startElectric = false;
                    Reset();
                    outOfRange = false;
                }
            }
        
        }
        electricPos.transform.position = new Vector2(moveX, moveY);
    }

    private void Reset()
    {
        timer = 0.0f;
    }

}
