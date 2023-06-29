/*
 * �쐬���ԁF2023/1/16
 * �쐬�ҁ@�F��
 * �������e�F�Q�[���C���X�^���X
 * 
 * �X�V���e�F2023/1/16 �Q�[���C���X�^���X�N���X������
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameInstance�N���X�́A�Q�[���̃Z�[�u�f�[�^���Ǘ�����N���X�ł��B
/// �܂��A�V���O���g���p�^�[�����g�p���āA�V�[���Ԃł̃Q�[���f�[�^�̕ێ����s���܂��B
/// </summary>
public class GameInstance : MonoBehaviour
{
    /// <summary>
    /// �Q�[���̃Z�[�u�f�[�^���i�[����\���̂ł��B
    /// SP�AHP�ADataOverScene�A�����ăv���C���[�̈ʒu����ێ����܂��B
    /// </summary>
    [System.Serializable]
    public struct SaveData
    {
        public int SP;
        public int HP;
        public int Score;

        public Vector2 Pos;
    }

    /// <summary>
    /// GameInstance�N���X�̃C���X�^���X��ێ�����ϐ��ł��B
    /// </summary>
    public static GameInstance m_instance;

    /// <summary>
    /// �Q�[���̃Z�[�u�f�[�^��ێ�����ϐ��ł��B
    /// </summary>
    [SerializeField]private SaveData m_saveData;

    /// <summary>
    /// ���݂̃Z�[�u�f�[�^���擾���郁�\�b�h�ł��B
    /// </summary>
    /// <returns>�Q�[���̃Z�[�u�f�[�^</returns>
    public SaveData GetSaveData()
    {
        return m_saveData;
    }

    /// <summary>
    /// �`�F�b�N�|�C���g��ݒ肷�郁�\�b�h�ł��B
    /// </summary>
    /// <param name="_pos">�ݒ肷��ʒu���</param>
    public void SetCheckPoint(Vector2 _pos)
    {
        m_saveData.Pos = _pos;
    }

    /// <summary>
    /// �Q�[���̃Z�[�u�f�[�^��ݒ肷�郁�\�b�h�ł��B
    /// </summary>
    /// <param name="_hp">�ݒ��HP</param>
    /// <param name="_sp">�ݒ��SP</param>
    /// <param name="_score">�ݒ�̃X�R�A</param>
    public void SetSaveData(int _hp = 100, int _sp = 0, int _score = 0)
    {
        m_saveData.HP = _hp;
        m_saveData.SP = _sp;
        m_saveData.Score = _score;
    }


    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

}
