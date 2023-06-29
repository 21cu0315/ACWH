using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class GameRuler : MonoBehaviour
{
    [SerializeField] Controller m_gamepad;
    public Player m_player;

    [Header("-HP-")]
    [SerializeField] Image m_HPGauge;
    [SerializeField] Image m_HPGaugeBase;
    [SerializeField] Animator m_HPGaugeAnim;
    [SerializeField] int m_HPRegainCnt = 0;
    [Header("-SP-")]
    [SerializeField] Image m_SPGauge;

    [Header("-EP-")]
    [SerializeField] Image m_EPGauge;
    [SerializeField] Image m_EPGaugeWarnig;
    [SerializeField] Image m_EPGaugeBase;
    [SerializeField] TextMeshProUGUI m_EPText;
    [SerializeField] TextMeshProUGUI m_EPReloadText;
    [SerializeField] Image m_EPReloadFrame;
    [SerializeField] Image m_EPReload;
    float m_EPAlpha = 0;
    float m_EPReloadAlpha = 0;


    [Header("-Chain-")]
    [SerializeField] Image m_chainUI;
    [SerializeField] TextMeshProUGUI chainUI;
    [SerializeField] int m_chain = 0;
     int chainCNT = 240;
    [SerializeField] Animator m_chainAnim;

    [Header("-DataOverScene-")]
    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] float m_score=0;
    [SerializeField] float m_scorePresent = 0;
    [SerializeField] Animator m_scoreAnim;

    [Header("-Hint-")]
    [SerializeField] Animator m_hintAnim;
    [SerializeField] Image m_hint;
    [SerializeField] GameObject m_hintObj;

    [SerializeField] Animator m_damageEff;
    [SerializeField] CameraTest m_camera;
    [SerializeField] Image m_blackBG;
    [SerializeField] GameObject m_playerSpwanerPrefab;
    [SerializeField] Transform m_playerSpwanPos;
    [SerializeField] UI_FollowCamera m_EPGaugeObj;


    public enum state
    {
        sceneIn,
        ingame,
        sceneOut
    };
    public state m_state = state.sceneIn;

    // Start is called before the first frame update
    void Awake()
    {

    }
    private void Start()
    {
     
    
        Application.targetFrameRate = 60;
        m_state = state.sceneIn;
    }
    public void SpwanPlayerSpwaner() 
    {
        Instantiate(m_playerSpwanerPrefab, m_playerSpwanPos.position,Quaternion.identity);

    }
    public void SetPlayer() 
    {
        m_player = FindObjectOfType<Player>();

        FindObjectOfType< Cinemachine.CinemachineVirtualCamera >().Follow = m_player.transform;
        m_EPGaugeObj.m_target = m_player.m_EPPos;
        m_player.SetGamePad(m_gamepad);
        var tmp_SoundManager = FindObjectOfType<SoundManager>();
        tmp_SoundManager.LoopAudio("BGM_Loop");
        tmp_SoundManager.SetAudioVol("BGM_Loop", 0.75f);
        m_player.m_SEManager = tmp_SoundManager;
        if (FindObjectOfType<DataOverScene>() != null)
        {
            m_score = FindObjectOfType<DataOverScene>().m_score;
            m_player.GetSetHP = FindObjectOfType<DataOverScene>().m_HP;
            m_player.GetSetSP = FindObjectOfType<DataOverScene>().m_SP;
            Destroy(FindObjectOfType<DataOverScene>().gameObject);
            m_SPGauge.fillAmount = m_player.GetSetSP / 100.0f;
            m_HPGauge.fillAmount = (float)m_player.GetSetHP / (float)m_player.GetHPMax;
            if (m_HPGauge.fillAmount > m_HPGaugeBase.fillAmount)
            {
                m_HPGaugeBase.fillAmount = m_HPGauge.fillAmount;
            }
            else if (m_HPGauge.fillAmount < m_HPGaugeBase.fillAmount)
            {
                if (m_HPRegainCnt == 0)
                {
                    m_HPGaugeBase.fillAmount -= 0.01f;
                }
            }
            m_HPGauge.fillAmount = (float)m_player.GetSetHP / (float)m_player.GetHPMax;


        }
        m_state = state.ingame;

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
            m_player.m_SPSkillOn = false;
        }
        switch (m_state) 
        {
            case state.sceneIn:
                break;

            case state.ingame:
             
                //debug
                //----------------------------------------------------------
                if (Gamepad.current.leftStickButton.wasPressedThisFrame)
                {
                    SceneManager.LoadScene(2);
                    Time.timeScale = 1;
                    m_player.m_SPSkillOn = false;
                    GameObject tmp_dataOverSceneObj = new GameObject();
                    tmp_dataOverSceneObj.name = "tmp_DataOver";
                    DataOverScene tmp_data = tmp_dataOverSceneObj.AddComponent<DataOverScene>();
                    tmp_data.m_score = m_score;
                    tmp_data.m_HP = m_player.GetSetHP;
                    tmp_data.m_SP = m_player.GetSetSP;
                    DontDestroyOnLoad(tmp_dataOverSceneObj);
                }
                //----------------------------------------------------------
                if (m_player.m_SPSkillOn)
                {
                    m_blackBG.enabled = true;
                    m_EPGauge.color = Color.clear;
                    m_EPGaugeBase.color = Color.clear;
                    m_EPGaugeWarnig.color = Color.clear;
                    m_EPText.color = Color.clear;
                    m_EPReloadFrame.color = Color.clear;
                    m_EPReload.color = Color.clear;
                    m_EPReloadText.color = Color.clear;
                    m_hintObj.SetActive(false);
                }
                else
                {
                    m_blackBG.enabled = false;
                    m_EPReloadFrame.color = new Color(1, 0.4f, 0.4f, m_EPReloadAlpha);
                    m_EPReload.color = new Color(1, 0.48f, 0.5f, m_EPReloadAlpha * 0.05f);
                    m_EPReloadText.color = new Color(Color.white.r, Color.white.g, Color.white.b, m_EPReloadAlpha);

                    m_EPGauge.color = new Color(m_EPGauge.color.r, m_EPGauge.color.g, m_EPGauge.color.b, m_EPAlpha);
                    m_EPGaugeBase.color = new Color(Color.white.r, Color.white.g, Color.white.b, m_EPAlpha);
                    m_EPGaugeWarnig.color = new Color(1, 0.25f, 0.25f, m_EPAlpha / 2f);
                    m_EPText.color = m_EPGaugeBase.color;
                    if (!m_hintObj.activeInHierarchy)
                    {
                        m_hintObj.SetActive(true);
                    }
                }
                break;
            case state.sceneOut:
                break;
        }
        //debug
        
    }
    private void FixedUpdate()
    {
        switch (m_state)
        {
            case state.sceneIn:
                break;

            case state.ingame:
                //m_camera.m_slowFollow = m_player.GetDashing;


                m_damageEff.SetBool("GetDamage", m_player.GetDamage());
                m_damageEff.SetFloat("HP", m_player.GetSetHP);



                m_EPGauge.fillAmount = m_player.GetSetEP / 50.0f;

                if (m_player.GetReload)
                {
                    m_EPGauge.color = Color.Lerp(new Color(1, 0f, 0f, m_EPGauge.color.a), new Color(0, 1, 0.95f, m_EPGauge.color.a), m_EPGauge.fillAmount * 0.75f);
                    m_EPReloadAlpha = Mathf.Abs(Mathf.Cos(Time.time * 2f)) * 0.75f;
                }
                else
                {
                    m_EPGauge.color = new Color(0, 1, 0.95f, m_EPGauge.color.a);
                    m_EPReloadAlpha -= 0.1f;
                }


                if (m_EPGauge.fillAmount == 1)
                {
                    m_EPAlpha -= 0.1f;
                }
                else
                {
                    m_EPAlpha += 0.1f;
                }
                m_EPAlpha = Mathf.Clamp(m_EPAlpha, 0, 1);
                m_EPReloadAlpha = Mathf.Clamp(m_EPReloadAlpha, 0, 1);




                m_SPGauge.fillAmount = m_player.GetSetSP / 100.0f;

                if (m_player.GetDamageCnt() != 0 && m_HPRegainCnt == 0)
                {
                    m_HPRegainCnt = 120;
                }
                if (m_HPRegainCnt > 0)
                {
                    m_HPRegainCnt--;
                }

                if (m_HPGauge.fillAmount > m_HPGaugeBase.fillAmount)
                {
                    m_HPGaugeBase.fillAmount = m_HPGauge.fillAmount;
                }
                else if (m_HPGauge.fillAmount < m_HPGaugeBase.fillAmount)
                {
                    if (m_HPRegainCnt == 0)
                    {
                        m_HPGaugeBase.fillAmount -= 0.01f;
                    }
                }
                m_HPGauge.fillAmount = (float)m_player.GetSetHP / (float)m_player.GetHPMax;

                if (m_chain > 1)
                {
                    chainUI.text = m_chain + " Chain +" + Mathf.Clamp(m_chain * 5, 10, 50);
                }
                else
                {
                    chainUI.text = m_chain + " Chain";
                }
                if (m_chain > 0)
                {
                    chainCNT--;
                    m_chainUI.enabled = true;
                }
                if (chainCNT <= 0)
                {
                    m_chain = 0;
                    chainCNT = 240;
                    m_chainUI.enabled = false;
                }
                m_chainUI.fillAmount = (float)chainCNT / 240f;

                if (m_scorePresent != m_score)
                {
                    m_scorePresent = Mathf.Lerp(m_scorePresent, m_score, Time.deltaTime * 10f);
                }
                if (m_scorePresent > 0)
                {
                    scoreUI.text = Mathf.Round(m_scorePresent).ToString();
                }
                if (m_player.Hint)
                {
                    m_hint.sprite = m_player.m_hintSprite;
                }

                m_chainAnim.SetBool("in", m_chain > 0);
                m_scoreAnim.SetBool("in", m_score > 0);
                m_HPGaugeAnim.SetBool("Damage", m_player.GetDamage());
                m_hintAnim.SetBool("On", m_player.Hint);

                break;
            case state.sceneOut:
                break;
        }
       
    }

    public void comboPlus()
    {
        m_chain++;
        chainCNT = 240;
        if (m_chain > 1) 
        {
            int _score = Mathf.Clamp(m_chain * 5,10, 50);
            m_score += _score;
        }
    }
    public void DealDamage(int _score)
    {
        m_gamepad.setHaptics(0.075f, 5f);
         m_score += _score;
        if (m_HPGauge.fillAmount < m_HPGaugeBase.fillAmount && m_player.LockOning())
        {
            m_player.GetSetHP++;
        }
    }
    
}
