using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI_Script : MonoBehaviour
{
    [SerializeField] Animator m_anim;
    public bool m_sceneEnd = false;
    private void Update()
    {
        m_anim.SetBool("SceneEnd", m_sceneEnd);
    }
    void SceneFadeIn() 
    {
        FindObjectOfType<GameRuler>().SpwanPlayerSpwaner();
    }
    void SceneFadeOut()
    {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
    }
}
