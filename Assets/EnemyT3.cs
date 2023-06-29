using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyT3 : EnemyBase
{
    float m_distance = 5f;

    //�ړ����邽�߂ɁA�N�_�ƏI�_��X���W�̕ϐ������O�ɗp�ӂ���
    [SerializeField] private Vector2 pointA;
    [SerializeField] private Vector2 pointB;

    //�G��Y���W
    [SerializeField] private float m_Ypos;

    //�ړ�����
    [SerializeField] private bool ToRightSide = true;

    //�^�C�}�[
    private float timer = 5f;
    [SerializeField] private float floatTimer = 0f;

    //�G�Q�Ɨp
    Rigidbody2D m_rb;



    private void Start()
    {
        //�N�_�ƏI�_��X���W��������
        pointA = new Vector2(transform.position.x - m_distance, 0 + transform.position.y);
        pointB = new Vector2(transform.position.x + m_distance, 0 + transform.position.y);

        //Y���W��������
        m_Ypos = transform.position.y;
        
        //�G���Q�Ƃ���
        m_rb = GetComponent<Rigidbody2D>();

        //�G�̈ʒu���N�_�ƏI�_�̐^�񒆂ɗp�ӂ���
        m_rb.position = Vector2.Lerp(pointA, pointB, 0.5f);
    }

    protected override void SubClass()
    {
        //�^�C�}�[����
        timer += Time.fixedDeltaTime;
        floatTimer += Time.fixedDeltaTime;

        //�E�Ɉړ����鎞
        if (ToRightSide)
        {
            //�v���C���[������������
            if (m_playerFound)
            {
                //�^�C�}�[�𑁂���
                timer += 0.05f*2f;
            }
            else 
            {
                //�^�C�}�[����
                timer += 0.05f;
            }
            //�^�C�}�[���؂�����
            if (timer >= 10)
            {
                //���Ɉړ�����
                ToRightSide = false;
            }
            //������ς���
            transform.localScale = new Vector3(-1, 1, 1);
        }
        //���Ɉړ����鎞
        else
        {
            //�v���C���[������������
            if (m_playerFound)
            {
                //�^�C�}�[�𑁂���
                timer -= 0.05f * 2f;
            }
            else
            {
                //�^�C�}�[����
                timer -= 0.05f;
            }
            //�^�C�}�[���؂�����
            if (timer <= 0)
            {
                //�E�Ɉړ�����
                ToRightSide = true;
            }
            //������ς���
            transform.localScale = Vector3.one;
        }
        //�ړ�����
        m_rb.MovePosition( new Vector2(Mathf.Lerp(pointA.x, pointB.x, timer / 10f), transform.position.y-0.25f));
    }

    //�R���W����������������
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        //�v���C���[�Ɠ���������
        if (collision.gameObject.CompareTag("Player"))
        {
            //�v���C���[����������
            m_playerFound = true;
        }
    }

    //�R���W�������I�������
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //�v���C���[��������܂���
            m_playerFound = false;
        }
    }
}
