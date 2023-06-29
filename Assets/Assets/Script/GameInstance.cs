/*
 * 作成時間：2023/1/16
 * 作成者　：董
 * 処理内容：ゲームインスタンス
 * 
 * 更新内容：2023/1/16 ゲームインスタンスクラスを完成
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameInstanceクラスは、ゲームのセーブデータを管理するクラスです。
/// また、シングルトンパターンを使用して、シーン間でのゲームデータの保持を行います。
/// </summary>
public class GameInstance : MonoBehaviour
{
    /// <summary>
    /// ゲームのセーブデータを格納する構造体です。
    /// SP、HP、DataOverScene、そしてプレイヤーの位置情報を保持します。
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
    /// GameInstanceクラスのインスタンスを保持する変数です。
    /// </summary>
    public static GameInstance m_instance;

    /// <summary>
    /// ゲームのセーブデータを保持する変数です。
    /// </summary>
    [SerializeField]private SaveData m_saveData;

    /// <summary>
    /// 現在のセーブデータを取得するメソッドです。
    /// </summary>
    /// <returns>ゲームのセーブデータ</returns>
    public SaveData GetSaveData()
    {
        return m_saveData;
    }

    /// <summary>
    /// チェックポイントを設定するメソッドです。
    /// </summary>
    /// <param name="_pos">設定する位置情報</param>
    public void SetCheckPoint(Vector2 _pos)
    {
        m_saveData.Pos = _pos;
    }

    /// <summary>
    /// ゲームのセーブデータを設定するメソッドです。
    /// </summary>
    /// <param name="_hp">設定のHP</param>
    /// <param name="_sp">設定のSP</param>
    /// <param name="_score">設定のスコア</param>
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
