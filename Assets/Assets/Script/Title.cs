using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] SceneObject m_nextScene;
    [SerializeField] GameObject[] m_otherObj;

    // Update is called once per frame
    private void Update()
    {
        if (Gamepad.current.circleButton.wasPressedThisFrame )
        {
            GetComponent<Animator>().SetBool("NextScene", true);
            FindObjectOfType<SoundManager>().PlayAudioOneShot("SE_Title01");

        }
    }
  
    void NextScene()
    {
       
            SceneManager.LoadScene(m_nextScene);
        for (int i = 0; i < m_otherObj.Length; i++)
        {
            if (m_otherObj[i] != null) 
            {
                Destroy(m_otherObj[i]);
            }
        }
    }
}
