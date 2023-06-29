using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponsIndex 
    {
        MirageStella=0,
        AnchorSlasher,
        IlluminatiSpark,
        MeteorStrike
    }
    [SerializeField] GameObject m_bulletPrefab;
    [SerializeField] GameObject m_slasherPrefab;
    [SerializeField] GameObject m_weapon;
    [SerializeField] SpriteRenderer m_weaponSR;
    [SerializeField] int m_atkCNT = 0;
    [SerializeField][Range(0,100)] int m_sp = 100;
    [Header("[ -シュート位置- ]")]
    [SerializeField] Transform m_shootPos;
    [SerializeField] WeaponsIndex m_WeaponsIndex;

    [SerializeField] List<GameObject> m_LockONSort = new List<GameObject> {};
    [SerializeField] GameObject m_LockONMarkPrefab;
    [SerializeField] List<GameObject> m_LockONMark = new List<GameObject> { null, null, null, null, null };
    [SerializeField] GameObject m_lockONBulletPrefab;
    [SerializeField] GameObject m_slashEffPrefab;
    [SerializeField] int m_lockOnCNT = 0;
    [SerializeField] int m_weaponPosCNT = -1;
    [SerializeField] int m_ASCNT = 5;
    [SerializeField] int m_ASComboCNT = 1;
    [SerializeField] LineRenderer m_Line;
    [SerializeField] Animator m_weaponAnim;
    Player m_player;


    // Start is called before the first frame update
    void Start()
    {
        m_WeaponsIndex = WeaponsIndex.MirageStella;
        m_player = GetComponent<Player>();
    }
    public List<GameObject> getLockOnSort()
    {
        return m_LockONSort;
    }
    public void addObj(GameObject _newObj)
    {
        if (m_LockONSort.Contains(_newObj))
        {
            //for (int i = 0; i < m_LockONSort.Count; i++)
            //{
            //    if (m_LockONSort[i] == _newObj)
            //    {
            //        m_LockONSort[i].GetComponent<EnemyBase>().m_beLockTimeCNT = 240;
            //    }
            //}
            Destroy(m_LockONMark[0]);
            m_LockONMark.RemoveRange(0,1);
            m_LockONSort.Remove(_newObj);
            m_LockONSort.Insert(0, _newObj);
            //m_LockONSort[0].GetComponent<EnemyBase>().m_beLockTimeCNT = 240;
            //m_LockONSort[0].GetComponent<EnemyBase>().m_beLocked = true;
            m_LockONMark.Insert(0, Instantiate(m_LockONMarkPrefab, m_LockONSort[0].transform.position, Quaternion.identity));
        }
        else
        {
            m_LockONSort.Insert(0, _newObj);
            //m_LockONSort[0].GetComponent<EnemyBase>().m_beLockTimeCNT = 240;
            //m_LockONSort[0].GetComponent<EnemyBase>().m_beLocked = true;
            m_LockONMark.Insert(0, Instantiate(m_LockONMarkPrefab, m_LockONSort[0].transform.position,Quaternion.identity));
        }
    }

    void Update()
    {
        
        

      
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            m_sp = 100;
        }
        
        if (Input.GetKeyDown(KeyCode.JoystickButton7)||Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            m_WeaponsIndex--;
            for (int i = 0; i < m_LockONMark.Count; i++)
            {
                Destroy(m_LockONMark[i]);
            }
            m_LockONMark.Clear();
            m_LockONSort.Clear();
        }
        if (m_WeaponsIndex < WeaponsIndex.MirageStella)
        {
            m_WeaponsIndex = WeaponsIndex.AnchorSlasher;
          
                m_weaponSR.transform.rotation = Quaternion.Euler(m_weapon.transform.rotation.eulerAngles.x, m_weapon.transform.rotation.eulerAngles.y, 90f * -m_player.GetFacing());

        }
        if (m_WeaponsIndex > WeaponsIndex.AnchorSlasher)
        {
            m_WeaponsIndex = WeaponsIndex.MirageStella;
        }
        switch (m_WeaponsIndex) 
        {
            case WeaponsIndex.MirageStella:
                if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyUp(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyUp(KeyCode.JoystickButton2) || m_atkCNT == 6)
                {
                    m_atkCNT = 0;
                }
                m_Line.positionCount = 0;

                m_weaponSR.transform.rotation = Quaternion.identity;
                while (m_LockONSort.Count > 5)
                {
                    m_LockONSort.Remove(m_LockONSort[5]);
                }
                while (m_LockONMark.Count > 5)
                {
                    Destroy(m_LockONMark[5]);
                    m_LockONMark.Remove(m_LockONMark[5]);
                }


                if (m_LockONSort.Count > 0)
                {
                    for (int i = 0; i < m_LockONSort.Count; i++)
                    {
                        if (m_LockONSort[i])
                        {
                            m_LockONMark[i].transform.position = m_LockONSort[i].transform.position;

                            //if (!m_LockONSort[i].GetComponent<EnemyBase>().m_beLocked || !m_LockONSort[i].TryGetComponent<EnemyBase>(out EnemyBase tmp_c))//!m_LockONSort[i].GetComponentInChildren<SpriteRenderer>().isVisible
                            //{
                            //    m_LockONSort.Remove(m_LockONSort[i]);
                            //    m_LockONMark.Remove(m_LockONMark[i]);
                            //    //Destroy(m_LockONMark[i]);
                            //}
                        }
                        else
                        {
                            m_LockONSort.Remove(m_LockONSort[i]);
                            Destroy(m_LockONMark[i]);
                            m_LockONMark.Remove(m_LockONMark[i]);
                        }
                    }
                }
                else 
                {
                    if (m_LockONMark.Count>0) 
                    {
                        for (int i = 0; i < m_LockONMark.Count; i++)
                        {
                            Destroy(m_LockONMark[i]);
                        }
                        m_LockONMark.Clear();
                    }
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.JoystickButton0) || m_lockOnCNT == 8)
                {
                    m_lockOnCNT = 0;
                }
                if (Input.GetKey(KeyCode.JoystickButton0) && m_player.m_canControll)
                {
                    if (m_lockOnCNT == 0)
                    {
                        var _player = GetComponent<Player>();
                        GameObject tmp_lockOnBullet = Instantiate(m_lockONBulletPrefab, m_shootPos.transform.position, Quaternion.identity);
                        tmp_lockOnBullet.transform.localScale = new Vector3(1 * (float)m_player.GetFacing(), 1, 1) * 3.0f;
                        var tmp_lockOnBulletBehave = tmp_lockOnBullet.GetComponent<Bullet>();
                        tmp_lockOnBulletBehave.setSpeed(40.0f);
                        if (Input.GetAxis("YInputL") != 0)
                        {
                           tmp_lockOnBulletBehave.setDirection(new Vector2(_player.GetFacing(), Input.GetAxis("YInputL")).normalized);
                            if (_player.GetFacing() > 0)
                            {
                                float tmp_rotZ = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis("YInputL"), 1);
                                tmp_lockOnBullet.transform.rotation = Quaternion.Euler(0, 0, (tmp_rotZ));
                            }
                            else
                            {
                                float tmp_rotZ = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis("YInputL"), 1);
                                tmp_lockOnBullet.transform.rotation = Quaternion.Euler(0, 0, -(tmp_rotZ));
                            }
                        }
                        else 
                        {
                            tmp_lockOnBulletBehave.setDirection(Vector2.right* _player.GetFacing());
                            tmp_lockOnBullet.transform.rotation = Quaternion.identity;
                        }
                        tmp_lockOnBulletBehave.setPlayer(_player);

                    }
                    m_lockOnCNT++;
                }

                //if (Input.GetKeyDown(KeyCode.JoystickButton2))
                //{
                //    for (int i = 0; i < m_LockONMark.Count; i++)
                //    {
                //        Destroy(m_LockONMark[i]);
                //    }
                //    m_LockONMark.Clear();
                //    m_LockONSort.Clear();
                //}

                if (Input.GetKey(KeyCode.JoystickButton2)) 
                {
                    if (m_atkCNT == 0 && m_sp >= 2)
                    {
                            MirageStella();
                    }
                    m_atkCNT++;
                }
                
                else if (Input.GetKey(KeyCode.JoystickButton5))
                {
                    if (m_atkCNT == 0 && m_sp >= 2)
                    {
                        if (m_LockONSort.Count <= 0)
                        {
                            MirageStella();
                        }
                        else
                        {
                            MirageStellaLock();
                        }
                        //m_sp -= 2;
                    }
                    m_atkCNT++;
                }
                
                break;
            case WeaponsIndex.AnchorSlasher:
            
                while (m_LockONSort.Count > 1)
                {
                    m_LockONSort.Remove(m_LockONSort[1]);
                }
                while (m_LockONMark.Count > 1)
                {
                    Destroy(m_LockONMark[1]);
                    m_LockONMark.Remove(m_LockONMark[1]);
                }
                if (m_LockONSort.Count > 0)
                {
                    for (int i = 0; i < m_LockONSort.Count; i++)
                    {
                        if (m_LockONSort[i])
                        {
                            m_LockONMark[i].transform.position = m_LockONSort[i].transform.position;
                            m_Line.positionCount = 2;
                            m_Line.SetPosition(0, transform.position);
                            m_Line.SetPosition(1, m_LockONSort[i].transform.position);
                            //if (!m_LockONSort[i].GetComponent<EnemyBase>().m_beLocked)//!m_LockONSort[i].GetComponentInChildren<SpriteRenderer>().isVisible
                            //{
                            //    m_LockONSort.Remove(m_LockONSort[i]);
                            //    Destroy(m_LockONMark[i]);
                            //    m_LockONMark.Remove(m_LockONMark[i]);
                            //    m_Line.positionCount = 0;
                            //}
                        }
                    }
                }
            
                if (m_player.m_canControll) 
                {
                    if (Input.GetKeyDown(KeyCode.JoystickButton2))
                    {
                        ASZeroLock();
                    }
                    else if (Input.GetKeyDown(KeyCode.JoystickButton5))
                    {

                        if (m_LockONSort.Count == 0)
                        {
                            ASZeroLock();
                        }
                        else
                        {
                            m_player.SetAnchorSlashCNT(0);
                            m_player.setDoubleJump(false);
                            m_Line.positionCount = 0;
                            if (Input.GetAxis("XInputL") == 0)
                            {
                                if (transform.position.x < m_LockONSort[m_LockONSort.Count - 1].transform.position.x)
                                {
                                    transform.position = new Vector2(m_LockONSort[m_LockONSort.Count - 1].transform.position.x - 2.0f, m_LockONSort[m_LockONSort.Count - 1].transform.position.y + 1f);
                                }
                                else
                                {
                                    transform.position = new Vector2(m_LockONSort[m_LockONSort.Count - 1].transform.position.x + 2.0f, m_LockONSort[m_LockONSort.Count - 1].transform.position.y + 1f);
                                }
                            }
                            else
                            {
                                if (Input.GetAxis("XInputL") > 0)
                                {
                                    transform.position = new Vector2(m_LockONSort[m_LockONSort.Count - 1].transform.position.x + 2.0f, m_LockONSort[m_LockONSort.Count - 1].transform.position.y + 1f);
                                }
                                else
                                {
                                    transform.position = new Vector2(m_LockONSort[m_LockONSort.Count - 1].transform.position.x - 2.0f, m_LockONSort[m_LockONSort.Count - 1].transform.position.y + 1f);
                                }
                            }
                            m_LockONSort[m_LockONSort.Count - 1].GetComponent<EnemyBase>().Damage(50);
                            GameObject tmp_SlashAtk = Instantiate(m_slashEffPrefab, m_LockONSort[m_LockONSort.Count - 1].transform.position, Quaternion.identity);
                            Destroy(tmp_SlashAtk, 1f);
                            tmp_SlashAtk.GetComponent<SlashAtk>().SetDamage(50);
                            for (int i = 0; i < m_LockONMark.Count; i++)
                            {
                                Destroy(m_LockONMark[i]);
                            }
                            m_LockONMark.Clear();
                            m_LockONSort.Clear();
                            m_weapon.transform.position = m_shootPos.transform.position;
                            m_weaponPosCNT = 0;
                            m_weaponSR.transform.rotation = Quaternion.Euler(m_weapon.transform.rotation.eulerAngles.x, m_weapon.transform.rotation.eulerAngles.y, 20f * -m_player.GetFacing());

                        }
                    }
              
                }
                if (m_atkCNT != -1) 
                {
                m_atkCNT++;
                }
                if (m_atkCNT >= 45) 
                {
                    m_atkCNT = -1;
                    m_ASComboCNT = 1;
                }
                if (Input.GetKeyDown(KeyCode.JoystickButton0) && m_player.m_canControll)
                {
                    if (m_ASCNT > 0)
                    {
                        var _player = GetComponent<Player>();
                        if (m_weaponPosCNT == -1)
                        {
                            m_weapon.transform.position = transform.position;
                        }
                        m_weaponPosCNT = 0;
                        GameObject tmp_slasher = Instantiate(m_slasherPrefab, m_weapon.transform.position, Quaternion.identity);
                        tmp_slasher.transform.localScale = new Vector3(1 * (float)m_player.GetFacing(), 1, 1) * 3.0f;
                        var tmp_lockOnslasherBehave = tmp_slasher.GetComponent<LockOnSlasher>();
                        tmp_lockOnslasherBehave.setSpeed(80.0f);
                        if (Input.GetAxis("YInputL") != 0)
                        {
                            if (_player.GetFacing() > 0)
                            {
                                tmp_lockOnslasherBehave.setDirection(new Vector2(_player.GetFacing(), Input.GetAxis("YInputL")).normalized);
                                float tmp_rotZ = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis("YInputL"), 1);
                                m_weaponSR.transform.rotation = Quaternion.Euler(0, 0, (tmp_rotZ));
                            }
                            else
                            {
                                tmp_lockOnslasherBehave.setDirection(new Vector2(_player.GetFacing(), Input.GetAxis("YInputL")).normalized);
                                float tmp_rotZ = Mathf.Rad2Deg * Mathf.Atan2(Input.GetAxis("YInputL"), 1);
                                m_weaponSR.transform.rotation = Quaternion.Euler(0, 0, -(tmp_rotZ));
                            }
                        }
                        else
                        {
                            tmp_lockOnslasherBehave.setDirection(Vector2.right * _player.GetFacing());
                            m_weaponSR.transform.rotation = Quaternion.identity;
                        }
                        tmp_slasher.transform.rotation = m_weaponSR.transform.rotation;
                        tmp_lockOnslasherBehave.setPlayer(_player);
                        Destroy(tmp_slasher, 0.5f);
                        //GameObject tmp_eff = Instantiate(m_player.GetDashPrefab(), m_weapon.transform.position, m_weaponSR.transform.rotation, m_weaponSR.transform);
                        //tmp_eff.transform.localScale = Vector3.one * 0.3f;
                        //Destroy(tmp_eff, 0.2f);
                        //m_ASCNT--;
                    }
                }
                //if (Input.GetKeyDown(KeyCode.JoystickButton2))
                //{
                //    for (int i = 0; i < m_LockONMark.Count; i++)
                //    {
                //        Destroy(m_LockONMark[i]);
                //    }
                //    m_LockONMark.Clear();
                //    m_LockONSort.Clear();
                //    m_Line.positionCount = 0;
                //}
                if (m_player.GetAnchorSlashCNT() == -1)
                {
                    //m_Line.positionCount = 0;

                }
                if (m_player.GetGrounded)
                {
                    m_ASCNT = 5;
                }
                if (m_player.GetAnchorSlashCNT() != -1)
                {
                    m_weapon.transform.position = m_shootPos.transform.position;
                    m_weaponPosCNT = 0;
                    m_weaponSR.transform.rotation = Quaternion.Euler(m_weapon.transform.rotation.eulerAngles.x, m_weapon.transform.rotation.eulerAngles.y, 20f * -m_player.GetFacing());


                }

                break;
        }

        m_weapon.transform.localScale = new Vector3(1 * (float)m_player.GetFacing(), 1, 1) * 3.0f;

        if (m_weaponPosCNT == -1)
        {
            var _weaponPos = new Vector3(transform.position.x - 1.25f * m_player.GetFacing(), transform.position.y + 1.5f + 0.4f * Mathf.Cos(Time.time), transform.position.z);
            m_weapon.transform.position = Vector2.Lerp(m_weapon.transform.position, _weaponPos, Time.deltaTime * 3.0f);
            if (m_player.GetFacing() == (int)Player.Facing.L)
            {
                m_weaponSR.transform.rotation = Quaternion.Lerp(m_weaponSR.transform.rotation, Quaternion.Euler(m_weapon.transform.rotation.eulerAngles.x, m_weapon.transform.rotation.eulerAngles.y, 90f), Time.deltaTime * 4.5f);

            }
            else
            {
                m_weaponSR.transform.rotation = Quaternion.Lerp(m_weaponSR.transform.rotation, Quaternion.Euler(m_weapon.transform.rotation.eulerAngles.x, m_weapon.transform.rotation.eulerAngles.y, -90f), Time.deltaTime * 4.5f);


            }

        }
        else
        {
            if (m_WeaponsIndex == WeaponsIndex.MirageStella)
            {
                m_weapon.transform.position = Vector2.Lerp(m_weapon.transform.position, new Vector3(transform.position.x - 1.5f * (float)m_player.GetFacing(), transform.position.y, transform.position.z), Time.deltaTime * 3.5f);
            }
            else
            {
                m_weapon.transform.position = Vector2.Lerp(m_weapon.transform.position, transform.position, Time.deltaTime * 3.5f);
            }
            m_weaponPosCNT++;
            if (m_weaponPosCNT >= 15)
            {

                m_weaponPosCNT = -1;
            }
        }

        m_weaponAnim.SetInteger("WeaponIndex", (int)m_WeaponsIndex);

      

    }

    private void ASZeroLock()
    {
        //近接攻撃
        if (m_atkCNT < 40 && m_ASComboCNT != -1)
        {
            m_atkCNT = 0;
            if (m_weaponPosCNT == -1)
            {
                m_weapon.transform.position = new Vector3(m_shootPos.position.x + 0.5f * m_player.GetFacing(), m_shootPos.position.y, m_shootPos.position.z);
            }
            m_weaponPosCNT = 0;
            switch (m_ASComboCNT)
            {
                case 1:
                    m_weaponSR.transform.rotation = Quaternion.Euler(-50, -30, -15f * -m_player.GetFacing());
                    break;
                case 2:
                    m_weaponSR.transform.rotation = Quaternion.Euler(60, -15, 150 * -m_player.GetFacing());
                    break;
                case 3:
                    m_weaponSR.transform.rotation = Quaternion.Euler(-12, 30, -45f * -m_player.GetFacing());
                    break;
            }
            m_weaponPosCNT = 0;

            m_ASComboCNT++;
            if (m_ASComboCNT > 3)
            {
                m_ASComboCNT = -1;
            }
            if (m_player.GetDashing)
            {
                m_ASComboCNT = -1;

                if (m_player.GetGrounded)
                {
                    m_weapon.transform.position = new Vector3(transform.position.x + 2f * m_player.GetFacing(), transform.position.y, transform.position.z);
                }
                else
                {
                    m_weapon.transform.position = transform.position;
                }
                GameObject tmp_SlashAtk = Instantiate(m_slashEffPrefab, m_weaponSR.transform.position, Quaternion.identity);
                Destroy(tmp_SlashAtk, 0.5f);

            }
            else
            {
                if (m_player.GetGrounded)
                {
                  
                    GameObject tmp_SlashAtk = Instantiate(m_slashEffPrefab, m_shootPos.transform.position, Quaternion.identity);
                    Destroy(tmp_SlashAtk, 0.5f);
                    m_atkCNT = 0;
                }
                else
                {
                    m_ASComboCNT = -1;
                    m_weapon.transform.position = transform.position;
                    GameObject tmp_SlashAtk = Instantiate(m_slashEffPrefab, m_weaponSR.transform.position, Quaternion.identity);
                    Destroy(tmp_SlashAtk, 0.5f);
                }
            }

        }
        else if (m_atkCNT > 45)
        {
            m_ASComboCNT = -1;
        }
    }

    private void MirageStella()
    {
        if (m_weaponPosCNT == -1) 
        {
        m_weapon.transform.position = new Vector3(transform.position.x - 1.5f * (float)m_player.GetFacing(), transform.position.y, transform.position.z);
        }
        m_weaponPosCNT = 0;

        GameObject tmp_bullet = Instantiate(m_bulletPrefab, m_weapon.transform.position, Quaternion.identity);
        tmp_bullet.transform.localScale = Vector3.one;
        var tmp_bulletTrail = tmp_bullet.GetComponent<TrailRenderer>();
        tmp_bulletTrail.startWidth = 0.125f;
        tmp_bulletTrail.endWidth = 0.125f;
        var tmp_bulletBehave = tmp_bullet.GetComponent<Lazer>();
        //tmp_bulletBehave.m_dir = GetComponent<Player>().GetFacing();
    }

    private void MirageStellaLock()
    {
        //m_miragePod[0].transform.position
        //追跡レーザー
        for(int i =0;i< m_LockONSort.Count;i++) 
        {
            if (m_LockONSort[i]) 
            {
                GameObject tmp_bullet = Instantiate(m_bulletPrefab, m_weapon.transform.position, Quaternion.identity);
                var tmp_bulletBehave = tmp_bullet.GetComponent<Lazer>();
                tmp_bulletBehave.m_timer = 0;
                tmp_bulletBehave.m_startPos = m_weapon.transform.position;
                float tmp_ranXPos = Random.Range(-8f, 8f);
                float tmp_ranYPos = Random.Range(-8f, 8f);
                Vector2 tmp_pos1 = new Vector2(transform.position.x + tmp_ranXPos, transform.position.y + tmp_ranYPos);
                Vector2 tmp_pos2 = new Vector2(m_LockONSort[i].transform.position.x + tmp_ranXPos, m_LockONSort[i].transform.position.y + tmp_ranYPos);
                tmp_bulletBehave.m_center1Pos = tmp_pos1;
                tmp_bulletBehave.m_center2Pos = tmp_pos2;
                tmp_bulletBehave.m_End = m_LockONSort[i];
                tmp_bulletBehave.m_EndPos = tmp_bulletBehave.m_End.transform.position;
            }
        }
    }
    public Weapon.WeaponsIndex GetWeaponsIndex() 
    {
        return m_WeaponsIndex;
    }
}
