using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : ProjectilesBase
{
    [SerializeField] private Collider2D firePos;
    [SerializeField] private Collider2D enemyPos;

    private float fireOriginPosX;

    private float fireOriginPosY;

    [SerializeField] private bool startFire;

    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        startFire = true;
        fireOriginPosX = firePos.transform.position.x;
        fireOriginPosY = firePos.transform.position.y;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {

        if (startFire == false)
        {
            firePos.transform.position = new Vector2(fireOriginPosX, fireOriginPosY);
            timer += Time.deltaTime;
            if (timer >= 4.0f)
            {
                startFire = true;
                Reset();
            }
        }
        else if (startFire == true) 
        {
            firePos.transform.position=new Vector2(enemyPos.transform.position.x -1.0f, enemyPos.transform.position.y);
            timer += Time.deltaTime;
            if (timer >= 2.0f)
            {
                startFire = false;
                Reset();
            }
        }
    }


    private void Reset()
    {
        timer = 0.0f;
    }
}
