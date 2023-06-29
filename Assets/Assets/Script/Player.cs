using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public enum Facing
    {
        L = -1,
        R = 1
    };
    Controller m_gamepad;
    public void SetGamePad(Controller _gamepad) { m_gamepad = _gamepad; }

    public Transform m_EPPos;

    Rigidbody2D m_rb2d;                                     // Rigidbody2D
    [SerializeField] Collider2D m_physicCol;


    [Header("[ -移動- ]")]
    [SerializeField] [Range(1, 100)] int m_speed = 10;        //移動速度
    [SerializeField] float m_xVelocity;                       //X軸の速度
    [SerializeField] float m_yVelocity;                       //Y軸の速度
    [SerializeField] float m_yVelocityDash;                       //Y軸の速度

    [Header("[ -ジャンプ- ]")]
    [SerializeField] LayerMask whichIsGround;                 //着地判定の対象の所属レイヤー
    [SerializeField] Transform m_groundingPos;                //着地判定の座標用
    [SerializeField] bool m_grounding;                        //着地してるか
    [SerializeField] bool m_doubleJump;                       //二段ジャンプ
    [SerializeField] float m_jumpForce;                       //ジャンプ力
    [SerializeField] float m_jumpingMaxCnt;           //ジャンプカウンターの最大限
    [SerializeField] float m_jumpingCnt;                                       //ジャンプ中のカウンター


    [Header("[ -ロックオンシステム- ]")]
    [SerializeField] GameObject m_target;
    [SerializeField] SpriteRenderer m_targetSprite;
    [SerializeField] GameObject m_LockOnMarkPrefab;
    [SerializeField] GameObject m_LockOnMark;
    [SerializeField] int m_LockOnCNT;
    [SerializeField] int m_LockOnFilpCNT;
    [SerializeField] bool m_LockOnFilping;
    [SerializeField] Collider2D m_LockOnCol;
    [SerializeField] float m_flipY;


    int m_shootCNT = 0;
    [SerializeField] Transform m_shootPos;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] GameObject m_LazerPrefab;
    [SerializeField] GameObject m_shootinfEffectPrefab;


    [Header("[ -状態- ]")]
    [SerializeField][Range(0, m_HPMax)] int m_HP;          //Hp
    [SerializeField][Range(0, 100)] float m_SP = 0;         
    [SerializeField][Range(0, 50)] float m_EP = 0;
    int m_EPCNT = 20;
    bool m_EPReload = false;
    int m_damageCnt = 0;
    const int m_HPMax = 100;                                //最大HP
    public bool m_canControll;                              //操作できるか
    //public bool m_damage;                                   //ダメージを受けているか
    [SerializeField] bool m_dash;
    [SerializeField] bool m_damage;
    [SerializeField] int m_dashCNT;
    [SerializeField] bool m_airDash;
    [SerializeField] int m_airDashCNT;
    bool m_wallKick;
    [SerializeField] int m_invciCNT =0;
    [SerializeField] Facing m_facing = Facing.R;
    [SerializeField] GameObject m_dashEffPrefab;
    [SerializeField] GameObject m_jumpEffPrefab;
    [SerializeField] AfterImage m_afterImage;
    RaycastHit2D m_ray0;
    RaycastHit2D m_ray1;
    RaycastHit2D m_ray2;

    [SerializeField] SpriteRenderer m_SR;

    [SerializeField] Animator m_anim;
    int m_AnchorSlashCNT = -1;

    [SerializeField]Cutter m_cutter;
    [SerializeField]GameObject m_cutterPrefab;
    [SerializeField]GameObject m_SPSkillPrefab;
    [SerializeField]GameObject m_SPSkill;
    [SerializeField] bool m_flipOver = false;
    public bool m_SPSkillOn = false;

    public SoundManager m_SEManager;

    void Awake()
    {
        m_rb2d = GetComponent<Rigidbody2D>();       
        m_HP = m_HPMax;                             
        m_canControll = true;
        //GameInstance.SaveData tmp_save = FindObjectOfType<GameInstance>().GetSaveData();
        //m_HP = tmp_save.HP;
        //m_SP = tmp_save.SP;
        //transform.position = tmp_save.Pos;
        m_grounding = true;
        m_cutter  = Instantiate(m_cutterPrefab,transform.position,Quaternion.identity).GetComponent<Cutter>();

    }


    void damageing()
    {
        if (m_canControll)
        {
            m_canControll = !m_canControll;
            m_rb2d.velocity = Vector2.zero;
            m_afterImage.On(false);
            m_rb2d.AddForce(new Vector2((int)m_facing * 200.0f * -1, 400f));
            m_rb2d.gravityScale = 6;
            m_dashCNT = 20;
            m_afterImage.On(false);
            m_dash = false;
            m_airDash = false;
            m_airDashCNT = 20;

        }
        m_damageCnt--;

        if ( m_grounding && m_damageCnt <= 0)
        {
            m_rb2d.velocity = Vector2.zero;
            m_xVelocity = 0;
            m_damageCnt = 0;
            m_damage = false;
            m_canControll = !m_canControll;
            m_invciCNT = 30;
        }

    }

    //物理処理はFixedUpdateの中で
    void FixedUpdate() 
    {
        if (!m_SPSkillOn) 
        {
            if (m_SP < 100) 
            {
                m_SP += Time.fixedDeltaTime*3f;
            }
            if (m_LockOnCNT > 0)
            {
                m_LockOnCNT--;
            }
            if (m_LockOnFilpCNT == -10 && !m_damage && m_LockOnFilping)
            {
                m_jumpingCnt = 0;
                m_LockOnFilping = false;
                m_flipOver = false;
            }
            if (m_LockOnCNT == 0)
            {
                if (m_target != null)
                {
                    m_target = null;
                }
            }

            //if (!m_EPReload)
            //{
            //   // m_EPCNT = 20;
            //}
            //else 
            //{
            if (m_EPCNT > 0)
            {
                m_EPCNT--;
                //if (m_EPReload) { m_EP = 0; }

            }
            if (m_EPCNT == 0)
            {
                if (!m_EPReload)
                { m_EP += 1.5f; }
                else
                {
                    m_EP += 0.75f;
                }
            }
            m_EP = Mathf.Clamp(m_EP, 0, 50);

            //}
            if (m_EP <= 0)
            {
                m_EPReload = true;
            }
            if (m_EPReload && m_EP == 50)
            {
                m_EPReload = false;
            }
            //ジャンプ中
            if (!m_grounding)
            {

                if (m_airDashCNT > -1)
                {
                    m_airDashCNT--;
                    m_jumpingCnt = 1;
                    m_dash = true;
                    m_dashCNT = 0;
                }
                else
                {
                    //jumpingCntの最大限と最小限を限制する
                    m_jumpingCnt = Mathf.Clamp(m_jumpingCnt, -m_jumpingMaxCnt * 2.0f, m_jumpingMaxCnt);
                    //毎フレーム、jumpingCntを1減らす
                    m_jumpingCnt--;
                    if (m_dash && !m_damage)
                    {
                        m_dashCNT = 0;
                        if (Gamepad.current.leftStick.x.ReadValue() > -0.5f && Gamepad.current.leftStick.x.ReadValue() < 0.5f)
                        {
                            m_rb2d.velocity = new Vector2(0, m_yVelocity);
                        }
                    }
                }


            }
            else
            {
                m_airDashCNT = -1;

                if (m_dash)
                {
                    m_dashCNT--;
                    if (m_dashCNT <= 0)
                    {
                        m_afterImage.On(false);
                        m_dashCNT = 20;

                        m_dash = false;
                    }
                }

            }
            if (!m_damage)
            {
                if (m_invciCNT > 0)
                {
                    m_invciCNT--;
                    if (m_invciCNT % 5 == 0)
                    {
                        m_SR.color = new Color(m_SR.color.r, m_SR.color.g, m_SR.color.b, 0.2f);
                    }
                    else
                    {
                        m_SR.color = new Color(m_SR.color.r, m_SR.color.g, m_SR.color.b, 1);
                    }
                }
                else
                {
                    m_SR.color = new Color(m_SR.color.r, m_SR.color.g, m_SR.color.b, 1);
                }
                if (m_dash && m_airDashCNT == -1)
                {
                    m_flipOver = false;

                    if (m_grounding)
                    {
                        m_LockOnCol.enabled = true;
                    }
                    else
                    {
                        m_LockOnCol.enabled = false;
                    }
                    if (Gamepad.current.leftStick.x.ReadValue() > -0.5f && Gamepad.current.leftStick.x.ReadValue() < 0.5f)
                    {
                        if (m_grounding)
                            m_rb2d.MovePosition(m_rb2d.position + new Vector2((float)m_facing * m_speed * 2f, m_yVelocity) * Time.fixedDeltaTime);
                    }
                    else
                    {
                        m_rb2d.MovePosition(m_rb2d.position + new Vector2(m_xVelocity * 2f, m_yVelocity) * Time.fixedDeltaTime);
                    }
                    m_rb2d.gravityScale = 6;

                }
                else if (m_airDashCNT > -1)
                {
                    m_LockOnCol.enabled = true;
                    m_rb2d.MovePosition(m_rb2d.position + new Vector2((float)m_facing * m_speed * 2f, m_yVelocityDash + Gamepad.current.leftStick.y.ReadValue() / 1f * 2f) * 1.15f * Time.fixedDeltaTime);
                    m_rb2d.gravityScale = 0;
                }
                else
                {

                    m_rb2d.gravityScale = 6;

                    if (m_LockOnFilpCNT > -10 && m_LockOnFilpCNT < 11)
                    {
                        if (m_flipOver)
                        {
                            m_rb2d.MovePosition(m_rb2d.position + new Vector2(m_speed * -(float)m_facing*1.5f, m_yVelocity * 1.5f) * 1.25f * Time.fixedDeltaTime);
                        }
                        else 
                        {
                            m_rb2d.MovePosition(m_rb2d.position + new Vector2(m_speed * -(float)m_facing, m_yVelocity * 1.5f) * 1.25f * Time.fixedDeltaTime);
                        }
                        m_LockOnFilpCNT--;
                    }

                    else
                    {
                        m_afterImage.On(false);
                        m_LockOnCol.enabled = false;
                        m_rb2d.MovePosition(m_rb2d.position + new Vector2(m_xVelocity, m_yVelocity) * Time.fixedDeltaTime);
                    }


                }
            }
        }
      
    }
    //オブジェクトを起動中に、毎フレームに処理する
    void Update()
    {
        if (!m_SPSkillOn)
        {
            m_SEManager.SetAudioVol("BGM_Loop", 0.75f);
            if (m_SPSkill)
            {
                Destroy(m_SPSkill);
            }
            if (!m_target)
            {
                if (m_LockOnMark)
                {
                    Destroy(m_LockOnMark);
                }
                m_LockOnCNT = 0;
                m_targetSprite = null;
            }
            else
            {
                if (m_LockOnMark)
                {
                    m_LockOnMark.transform.position = m_target.transform.position;
                }
                if (!m_targetSprite.isVisible)
                {
                    if (m_LockOnMark)
                    {
                        Destroy(m_LockOnMark);
                    }
                    m_LockOnCNT = 0;
                    m_targetSprite = null;
                    m_target = null;

                }
                //foreach (SpriteRenderer _sr in m_target.GetComponent<EnemyBase>().m_SR) 
                //{
                //    if (!_sr.isVisible)
                //    {
                //        if (m_LockOnMark)
                //        {
                //            Destroy(m_LockOnMark);
                //        }
                //        m_LockOnCNT = 0;
                //        m_target = null;
                //        break;
                //    }
                //}
            }
            if (m_damage)
            {
                damageing();
            }
            else
            {
                if (m_canControll)
                {
                    shooting();
                    movement();
                    Dash();
                    if (((Gamepad.current.leftShoulder.wasPressedThisFrame && !m_grounding) || Gamepad.current.rightShoulder.wasPressedThisFrame) && m_EPReload)
                    {
                        m_gamepad.setHaptics(0.015f, 5f);
                        m_SEManager.PlayAudioOneShot("SE_Reloading");
                    }
                    if (Gamepad.current.buttonNorth.wasPressedThisFrame && m_SP >= 100)
                    {
                        m_SPSkill = Instantiate(m_SPSkillPrefab, FindObjectOfType<Canvas>().transform);
                        Time.timeScale = 0;
                        m_SPSkillOn = true;
                        m_SP = 0;
                    }
                }
                else
                {
                    m_rb2d.velocity = Vector2.zero;
                }
                facingCheck();
            }
            if (m_AnchorSlashCNT != -1)
            {
                m_AnchorSlashCNT++;
                m_damage = false;
                m_canControll = false;
                m_yVelocity = 0;
                m_rb2d.simulated = false;
                m_grounding = false;
                if (m_AnchorSlashCNT >= 20)
                {
                    m_dash = true;
                    m_dashCNT = 20;
                    m_afterImage.On(true);
                    m_AnchorSlashCNT = -1;
                    m_canControll = true;
                    m_rb2d.simulated = true;
                    m_rb2d.gravityScale = 2.5f;
                }
            }
            else
            {
                Jump();
            }

            HP();
            Cutter();




            m_anim.SetBool("XVec isnt 0", m_xVelocity > 0.2f || m_xVelocity < -0.2f);
            m_anim.SetFloat("YVec", m_yVelocity);
            float tmp_shoot;
            if (
                (m_target && !m_EPReload && m_EP > 0 && (Gamepad.current.buttonWest.isPressed || Gamepad.current.rightShoulder.isPressed) && !m_damage) ||
                ((Gamepad.current.buttonWest.isPressed || Gamepad.current.rightShoulder.isPressed && !m_EPReload && m_EP > 0) && !m_damage)
                )
            {
                tmp_shoot = 0;
            }

            else
            {
                tmp_shoot = 1f;
            }

            m_anim.SetFloat("Shooting", tmp_shoot);
            m_anim.SetInteger("DashCNT", m_dashCNT);
            m_anim.SetInteger("AirDashCNT", m_airDashCNT);

            m_anim.SetInteger("ASCNT", m_AnchorSlashCNT);

            m_anim.SetBool("Grounded", m_grounding);
            m_anim.SetBool("Damage", m_damage);
            m_anim.SetBool("Dashing", m_dash);
            m_anim.SetBool("WallKick", m_wallKick);
            m_anim.SetBool("SPSkill", m_SPSkillOn);
            m_anim.SetBool("Fliping", m_LockOnFilpCNT > -10);

        }
        else 
        {
            m_SEManager.SetAudioVol("BGM_Loop", 0.4f);
        }
        if (m_HP <= 0)
        {
            m_HP = 0;
            m_canControll = false;
            m_damage = false;
            m_damageCnt = 0;
        m_anim.SetBool("HP<0", true);
            m_xVelocity = 0;
            m_yVelocity = 0;
            m_rb2d.velocity = Vector2.zero;
            m_rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(this);
        }
    }

    private void Cutter()
    {
        if (Gamepad.current.rightShoulder.isPressed && !m_EPReload && !m_damage)
        {
            m_SEManager.PlayWhileNotPlaying("SE_Cutter");
        }
        else 
        {
            m_SEManager.StopAudio("SE_Cutter");
        }
        if (Gamepad.current.rightShoulder.isPressed && !m_EPReload && m_cutter.m_timer == 20 &&!m_damage)
        {
            m_cutter.gameObject.SetActive(true);
            m_cutter.m_dir = (int)m_facing;
            m_cutter.m_target = m_target;
            m_cutter.gameObject.transform.position = new Vector2(transform.position.x + 2.5f * (int)m_facing, transform.position.y);
            m_gamepad.setHaptics(1, 1);
            

            m_EP -= 0.45f;
            if (m_EPCNT <= 15)
            {
                m_EPCNT = 15;
            }
        }
        else if (!Gamepad.current.rightShoulder.isPressed || m_EPReload || m_cutter.m_timer != 20 ||m_damage)
        {
            if (m_cutter.GetCharge() && m_cutter.gameObject.activeSelf)
            {
                if (m_cutter.m_timer > 0)
                {
                    m_cutter.m_timer--;
                    if (m_cutter.m_target)
                    {
                        if (m_cutter.gameObject.transform.position.x > m_cutter.m_target.transform.position.x)
                        {
                            m_cutter.gameObject.transform.position = Vector2.MoveTowards(m_cutter.gameObject.transform.position,new Vector2(m_cutter.m_target.transform.position.x + 0.75f, m_cutter.m_target.transform.position.y) , 0.5f);

                        }
                        else 
                        {
                            m_cutter.gameObject.transform.position = Vector2.MoveTowards(m_cutter.gameObject.transform.position,new Vector2(m_cutter.m_target.transform.position.x - 0.75f, m_cutter.m_target.transform.position.y) , 0.5f);
                        }
                    }
                    else 
                    {
                    m_cutter.gameObject.transform.position = new Vector2(m_cutter.gameObject.transform.position.x + 0.35f * m_cutter.m_dir, m_cutter.gameObject.transform.position.y);
                    }
                }
            }
            else
            {

                m_cutter.gameObject.SetActive(false);
                m_cutter.m_timer = 20;

            }
        }
    }

    void Dash()
    {
        if (Gamepad.current.leftShoulder.wasPressedThisFrame) 
        {
            if (m_grounding)
            {
                if (Gamepad.current.leftStick.y.ReadValue() <= 0.85f)
                {
                    m_dash = true;
                    m_dashCNT = 20;
                    m_afterImage.On(true);
                    Destroy(Instantiate(m_dashEffPrefab, m_SR.transform.position ,Quaternion.identity),  1f);
                    m_SEManager.PlayAudioOneShot("SE_Dash");

                    m_gamepad.setHaptics(0.2f, 2f);
                }
                else 
                {
                    if (m_EP > 0 && !m_EPReload  )
                    {
                        
                        m_airDashCNT = 25;
                        m_afterImage.On(true);

                        Destroy(Instantiate(m_dashEffPrefab, m_SR.transform.position, Quaternion.identity), 1f);
                        m_SEManager.PlayAudioOneShot("SE_Dash");

                        m_EP -= 10;
                        m_EPCNT = 90;
                        m_yVelocityDash = Gamepad.current.leftStick.y.ReadValue() * 6f;
                        m_grounding = false;
                        m_jumpingCnt = m_jumpingMaxCnt;
                        m_gamepad.setHaptics(0.2f, 2f);

                    }
                }
                   
            }
            else 
            {
                if (m_EP > 0 && !m_EPReload && !m_LockOnFilping ) 
                {
                    
                    m_airDashCNT = 25;
                    m_afterImage.On(true);
                    Destroy(Instantiate(m_dashEffPrefab, m_SR.transform.position, Quaternion.identity), 1f);
                    m_SEManager.PlayAudioOneShot("SE_Dash");

                    m_EP -= 10;
                    m_EPCNT = 90;
                    m_yVelocityDash = Gamepad.current.leftStick.y.ReadValue() *6f;
                    m_gamepad.setHaptics(0.2f, 5f);

                }

            }
           
        }
    }
    void Jump()
    {
        if (m_airDashCNT > -1 && m_airDashCNT < 24) 
        {
            
                Vector3 tmp_pos1Air = new Vector3(m_physicCol.bounds.center.x, m_groundingPos.position.y, m_groundingPos.position.z);
            Vector3 tmp_pos2Air = new Vector3(m_physicCol.bounds.center.x, m_physicCol.bounds.max.y, m_groundingPos.position.z);

            Vector3 tmp_pos3Air = new Vector3(m_physicCol.bounds.center.x, m_physicCol.bounds.center.y, m_groundingPos.position.z);

            m_ray1 = Physics2D.Raycast(tmp_pos1Air, Vector2.down, 0.15f, whichIsGround);
            m_ray2 = Physics2D.Raycast(tmp_pos2Air, Vector2.up, 0.15f, whichIsGround);


            Debug.DrawRay(tmp_pos1Air, Vector2.down * 0.15f, Color.green, Time.deltaTime);
                Debug.DrawRay(tmp_pos2Air, Vector2.down * 0.15f, Color.green, Time.deltaTime);



            if (m_ray1 || m_ray2 || Physics2D.Raycast(tmp_pos1Air, Vector2.right * (float)m_facing, 0.475f, whichIsGround) || Physics2D.Raycast(tmp_pos2Air, Vector2.right * (float)m_facing, 0.475f, whichIsGround)|| Physics2D.Raycast(tmp_pos3Air, Vector2.right * (float)m_facing, 0.475f, whichIsGround))   
            {
                m_airDashCNT = -1;
            }  
        }
        //落下中、又は着地中に、着地判定を起動させる
        if (m_jumpingCnt <= 0)
        {
            Vector3 tmp_pos0 = new Vector3(m_physicCol.bounds.min.x, m_groundingPos.position.y, m_groundingPos.position.z);
            Vector3 tmp_pos1 = new Vector3(m_physicCol.bounds.center.x , m_groundingPos.position.y, m_groundingPos.position.z);
            Vector3 tmp_pos2 = new Vector3(m_physicCol.bounds.max.x, m_groundingPos.position.y, m_groundingPos.position.z);
            m_ray0 = Physics2D.Raycast(tmp_pos0, Vector2.down, 0.15f, whichIsGround);
            m_ray1 = Physics2D.Raycast(tmp_pos1, Vector2.down, 0.15f, whichIsGround);
            m_ray2 = Physics2D.Raycast(tmp_pos2, Vector2.down, 0.15f, whichIsGround);

            Debug.DrawRay(tmp_pos0, Vector2.down * 0.15f, Color.green, Time.deltaTime);
            Debug.DrawRay(tmp_pos1, Vector2.down * 0.15f, Color.green, Time.deltaTime);
            Debug.DrawRay(tmp_pos2, Vector2.down * 0.15f, Color.green, Time.deltaTime);

            m_grounding = m_ray0 || m_ray1 || m_ray2;
           

        }
        //着地中に、Y軸の速度とジャンプカウンターを0に
        //ジャンプボタンを押したら、ジャンプカウンターを最大限に
        if (m_grounding)
        {
            //if (m_yVelocity <= -14f)
            //{
            //    m_gamepad.setHaptics(0, 1);
            //    m_EP += 20;
            //    Debug.Log("groundRecover");
            //    m_gamepad.setHaptics(0.025f, 5f);

            //}
            m_airDash = false;
            m_doubleJump = false;
            m_jumpingCnt = 0.0f;
            m_yVelocity = 0;
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                m_grounding = false;
                m_jumpingCnt = m_jumpingMaxCnt;
            }
       
        }
        //ジャンプ中
        if (!m_grounding)
        {
            if (m_jumpingCnt != 0)
            {
                m_yVelocity = m_jumpingCnt / m_jumpingMaxCnt * m_jumpForce;
            
                if (m_jumpingCnt > 0)
                {
                    
                    if ((Gamepad.current.buttonSouth.wasReleasedThisFrame && !m_doubleJump && !m_LockOnFilping) || m_damage)
                    {
                        m_jumpingCnt = 0.0f;
                        m_yVelocity = 0;
                    }
                }
                else 
                {
                    m_yVelocity = m_jumpingCnt / m_jumpingMaxCnt * m_jumpForce * 1.25f;
                }
                   
                
                
            }
            else 
            {
                m_yVelocity = 0;
            }
            //else 
            //{
            //    m_yVelocity = m_rb2d.velocity.y;
            //}
         
            Vector3 tmp_posC0 = new Vector3(m_physicCol.bounds.center.x, m_groundingPos.position.y, m_groundingPos.position.z);
            m_wallKick = Physics2D.Raycast(tmp_posC0, Vector2.right * (float)m_facing, 0.475f, whichIsGround) && ((m_xVelocity > 0 && m_facing == Facing.R) || (m_xVelocity < 0 && m_facing == Facing.L) ) && Gamepad.current.buttonSouth.wasPressedThisFrame;
            if (!m_damage && m_jumpingCnt < m_jumpingMaxCnt - 10f && !m_LockOnFilping && m_airDashCNT == -1)
            {

                //壁ジャンプ
                if (m_wallKick)
                {
                    m_doubleJump = false;
                    m_rb2d.velocity = Vector2.zero;
                    m_rb2d.AddForce(new Vector2(2750 * -(float)m_facing, 0));
                    m_jumpingCnt = m_jumpingMaxCnt;
                    m_gamepad.setHaptics(0.025f, 5f);
                    m_SEManager.PlayAudioOneShot("SE_WallKick");


                }
                else
                {
                    //二段ジャンプ
                    if (!m_doubleJump && m_jumpingCnt != m_jumpingMaxCnt && Gamepad.current.buttonSouth.wasPressedThisFrame)
                    {
                        m_doubleJump = true;
                        m_jumpingCnt = m_jumpingMaxCnt;
                        Destroy(Instantiate(m_jumpEffPrefab, transform.position, Quaternion.identity, transform), 1f);
                        m_gamepad.setHaptics(0.025f, 5f);
                        m_SEManager.PlayAudioOneShot("SE_Dash");


                    }

                }
            }
            if (m_airDashCNT > -1 && m_airDashCNT <23 && Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                m_airDashCNT = -1;
                m_rb2d.gravityScale = 6;
            }
        }
    }
    void movement()
    {

        if (m_canControll && !m_LockOnFilping) 
        {

            if (Gamepad.current.leftStick.x.ReadValue() != 0)
            {
                if (Gamepad.current.leftStick.x.ReadValue() > 0.5f)
                {
                    m_xVelocity = m_speed;
                    m_facing = Facing.R;
                }
                else if (Gamepad.current.leftStick.x.ReadValue() < -0.5f)
                {
                    m_xVelocity = -m_speed;
                    m_facing = Facing.L;
                }
            }
            else 
            {
                    m_xVelocity = 0;
            }
        }
    }
    void facingCheck()
    {
        if (m_LockOnFilpCNT == -10) 
        {
            //向きによって、弾の発射方向と、ロックマンの画像を反転させる
            switch (m_facing)
            {
                case Facing.L:
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    break;
                case Facing.R:
                    transform.localScale = Vector3.one;
                    break;
            }
        }
        
    }
    void shooting()
    {

        if (Gamepad.current.buttonWest.wasPressedThisFrame || Gamepad.current.buttonWest.wasReleasedThisFrame || m_shootCNT == 8)
        {
            m_shootCNT = 0;
        }


        if (Gamepad.current.buttonWest.isPressed )
        {
            if (m_shootCNT == 0)
            {
                if (m_target && m_EP > 0 && !m_EPReload)
                {
                    m_SEManager.PlayAudioOneShot("SE_PlayerShot");

                    GameObject tmp_lazer = Instantiate(m_LazerPrefab, m_shootPos.position, Quaternion.identity);
                    
                        Destroy(Instantiate(m_shootinfEffectPrefab, m_shootPos.position, Quaternion.identity),1f);
                    var tmp_lazerBehave = tmp_lazer.GetComponent<Lazer>();
                    tmp_lazerBehave.m_timer = 0;
                    tmp_lazerBehave.m_startPos = m_shootPos.position;
                    float tmp_ranXPos = Random.Range(-8f, 8f);
                    float tmp_ranYPos = Random.Range(-8f, 8f);
                    Vector2 tmp_pos1 = new Vector2(transform.position.x + tmp_ranXPos, transform.position.y + tmp_ranYPos);
                    Vector2 tmp_pos2 = new Vector2(m_target.transform.position.x + tmp_ranXPos, m_target.transform.position.y + tmp_ranYPos);
                    tmp_lazerBehave.m_center1Pos = tmp_pos1;
                    tmp_lazerBehave.m_center2Pos = tmp_pos2;
                    tmp_lazerBehave.m_End = m_target;
                    tmp_lazerBehave.m_EndPos = m_target.transform.position;
                    m_EP -= 0.75f;
                    if (m_EPCNT <= 20)
                    {
                        m_EPCNT = 20;
                    }
                }
                else if (!m_target ||( m_target &&  m_EPReload))
                {
                    m_SEManager.PlayAudioOneShot("SE_PlayerShot");

                    GameObject tmp_Bullet = Instantiate(m_BulletPrefab, m_shootPos.transform.position, Quaternion.identity);
                    Destroy(Instantiate(m_shootinfEffectPrefab, tmp_Bullet.transform.position, Quaternion.identity), 1f);

                    tmp_Bullet.transform.localScale = new Vector3(1 * (int)m_facing, 1, 1) * 3.0f;
                    var tmp_BulletBehave = tmp_Bullet.GetComponent<Bullet>();
                    tmp_BulletBehave.setSpeed(25.0f);

                   
                        tmp_BulletBehave.setDirection(Vector2.right * (float)m_facing);
                        tmp_Bullet.transform.rotation = Quaternion.identity;
                    
                    //else if(Gamepad.current.rightTrigger.isPressed && m_grounding && !m_dash &&!m_airDash)
                    //{

                    //    tmp_BulletBehave.setDirection(new Vector2((float)m_facing, Gamepad.current.leftStick.y.ReadValue()).normalized);
                    //    if ((float)m_facing > 0)
                    //    {
                    //        float tmp_rotZ = Mathf.Rad2Deg * Mathf.Atan2(Gamepad.current.leftStick.y.ReadValue(), 1);
                    //        tmp_BulletBehave.transform.rotation = Quaternion.Euler(0, 0, (tmp_rotZ));
                    //    }
                    //    else
                    //    {
                    //        float tmp_rotZ = Mathf.Rad2Deg * Mathf.Atan2(Gamepad.current.leftStick.y.ReadValue(), 1);
                    //        tmp_BulletBehave.transform.rotation = Quaternion.Euler(0, 0, -(tmp_rotZ));
                    //    }
                    //}
                  
                    
                    tmp_BulletBehave.setPlayer(this);
                }



            }

            m_shootCNT++;
        }




    }
    void HP()
    {
        m_HP = Mathf.Clamp(m_HP, 0, m_HPMax);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hint"))
        {
            Hint = true;
            m_hintSprite = collision.GetComponent<Tips>().m_tips;
        }
        if (collision.gameObject.CompareTag("LockCol") && !m_damage)
        {
            if (m_airDashCNT != -1)
            {
                if (m_LockOnMark)
                {
                    Destroy(m_LockOnMark);
                }
                m_target = collision.transform.parent.gameObject;
                m_targetSprite = m_target.GetComponent<EnemyBase>().m_SR;
                m_SEManager.PlaySELockOn();

                if (gameObject.transform.position.y > m_target.transform.position.y + 0.5f)
                {
                    if (gameObject.transform.position.x < m_target.transform.position.x)
                    {
                        if (Gamepad.current.leftStick.x.ReadValue() > 0)
                        {
                            m_facing = Facing.L;
                            m_flipOver = true;
                        }
                        else
                        {
                            m_facing = Facing.R;
                            m_flipOver = false;

                        }
                    }
                    else
                    {
                        if (Gamepad.current.leftStick.x.ReadValue() < 0)
                        {
                            m_facing = Facing.R;
                            m_flipOver = true;

                        }
                        else
                        {
                            m_facing = Facing.L;
                            m_flipOver = false;

                        }
                    }
                    switch (m_facing)
                    {
                        case Facing.L:
                            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                            break;
                        case Facing.R:
                            transform.localScale = Vector3.one;
                            break;
                    }
                }
                m_LockOnMark = Instantiate(m_LockOnMarkPrefab, m_target.transform.position, Quaternion.identity);
                m_LockOnFilpCNT = 10;
                m_LockOnFilping = true;
                m_LockOnCNT = 240;
                m_airDashCNT = -1;
                m_rb2d.velocity = Vector2.zero;
                m_jumpingCnt = 10;
                m_doubleJump = false;
                m_grounding = false;
                m_dash = false;
                m_EP += 5;
                m_gamepad.setHaptics(0.2f, 10f);
                m_EPCNT = 0;

            }

            else if (m_dash && m_grounding)
            {
                if (m_LockOnMark)
                {
                    Destroy(m_LockOnMark);
                }
                m_target = collision.transform.parent.gameObject;
                m_targetSprite = m_target.GetComponent<EnemyBase>().m_SR;
                m_SEManager.PlaySELockOn();
                m_LockOnMark = Instantiate(m_LockOnMarkPrefab, m_target.transform.position, Quaternion.identity);
                m_LockOnFilpCNT = 10;
                m_LockOnFilping = true;
                m_LockOnCNT = 240;
                m_dash = false;
                m_dashCNT = 0;
                m_rb2d.velocity = Vector2.zero;
                m_jumpingCnt = 10;
                m_grounding = false;
                m_gamepad.setHaptics(0.2f, 10f);
                m_EPCNT = 0;
            }
            else { return; }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hint"))
        {
            Hint = false;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (m_damage) { return; }
        if (collision.gameObject.CompareTag("LockCol") )
        {
            if (m_airDashCNT != -1)
            {
                if (m_LockOnMark)
                {
                    Destroy(m_LockOnMark);
                }
                m_target = collision.transform.parent.gameObject;
                m_targetSprite = m_target.GetComponent<EnemyBase>().m_SR;
                if (gameObject.transform.position.y > m_target.transform.position.y + 0.5f) 
                {
                    if (gameObject.transform.position.x < m_target.transform.position.x)
                    {
                        if (Gamepad.current.leftStick.x.ReadValue() > 0)
                        {
                            m_facing = Facing.L;
                            m_flipOver = true;
                        }
                        else
                        {
                            m_facing = Facing.R;
                            m_flipOver = false;

                        }
                    }
                    else
                    {
                        if (Gamepad.current.leftStick.x.ReadValue() < 0)
                        {
                            m_facing = Facing.R;
                            m_flipOver = true;

                        }
                        else
                        {
                            m_facing = Facing.L;
                            m_flipOver = false;

                        }
                    }
                    switch (m_facing)
                    {
                        case Facing.L:
                            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                            break;
                        case Facing.R:
                            transform.localScale = Vector3.one;
                            break;
                    }
                }
                m_SEManager.PlaySELockOn();

                m_LockOnMark = Instantiate(m_LockOnMarkPrefab, m_target.transform.position, Quaternion.identity);
                m_LockOnFilpCNT = 10;
                m_LockOnFilping = true;
                m_LockOnCNT = 240;
                m_airDashCNT = -1;
                m_rb2d.velocity = Vector2.zero;
                m_jumpingCnt = 10;
                m_grounding = false;
                m_dash = false;
                m_EP += 5;
                m_gamepad.setHaptics(0.2f, 10f);
                m_EPCNT = 0;
            }

            else if (m_dash && m_grounding)
            {
                if (m_LockOnMark)
                {
                    Destroy(m_LockOnMark);
                }
                m_target = collision.transform.parent.gameObject;
                m_targetSprite = m_target.GetComponent<EnemyBase>().m_SR;
                m_SEManager.PlaySELockOn();

                m_LockOnMark = Instantiate(m_LockOnMarkPrefab, m_target.transform.position, Quaternion.identity);
                m_LockOnFilpCNT = 10;
                m_LockOnFilping = true;
                m_LockOnCNT = 240;
                m_dash = false;
                m_dashCNT = 0;
                m_rb2d.velocity = Vector2.zero;
                m_jumpingCnt = 10;
                m_grounding = false;
                m_gamepad.setHaptics(0.2f, 10f);
                m_EPCNT = 0;

            }

        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss")) 
        {
            if (m_invciCNT == 0 &&!m_LockOnFilping)
            {
                int tmp_damage = 10;
                m_damage = true;
                m_damageCnt = 25;
                m_HP -= tmp_damage;
                m_gamepad.setHaptics(0.25f, 15f);
                m_SEManager.PlayAudioOneShotWhileNotPlaying("SE_Player_damage");


            }
        }
    }
    public int GetSetHP
    {
        get { return m_HP; }
        set { m_HP = value; }
    }
    public float GetSetSP
    {
        get { return m_SP; }
        set { m_SP = value; }
    }
    public float GetSetEP
    {
        get { return m_EP; }
        set { m_EP = value; }
    }
    public bool GetGrounded
    {
        get { return m_grounding; }
    }
    public int GetHPMax
    {
        get { return m_HPMax; }
    }
    public bool GetReload
    {
        get { return m_EPReload; }
    }
    public bool GetDashing
    {
        get { return m_dash||m_airDash; }
    }
    public Rigidbody2D GetRigibody2D
    {
        get { return m_rb2d; }
    }
 
    public int GetFacing()
    {
        return (int)m_facing;
    }
    public void SetAnchorSlashCNT( int _CNT)
    {
        m_AnchorSlashCNT = _CNT;
    }
    public int GetAnchorSlashCNT()
    {
       return  m_AnchorSlashCNT;
    }
    public bool GetDamage()
    {
        return m_damage;
    }
    public int GetDamageCnt()
    {
        return m_damageCnt;
    }

    public void death()
    {
        Destroy(this.gameObject);
    }
    public void setDoubleJump(bool _doubleJump)
    {
        m_doubleJump = _doubleJump;
    }
    public bool LockOning() 
    {
        return m_target;
    }
    public Sprite m_hintSprite;
    bool m_hint = false;
    public bool Hint 
    {
        get { return m_hint; }
        set { m_hint = value; }
    }

}
