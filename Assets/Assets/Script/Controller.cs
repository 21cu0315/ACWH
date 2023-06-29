using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public  class  Controller : MonoBehaviour
{
    public Gamepad m_gamepad;
    // Update is called once per frame
    void Update()
    {
        m_gamepad = Gamepad.current;
        
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            m_powerController--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_powerController++;
        }
        m_powerController = Mathf.Clamp(m_powerController, 1, 10);

        if (m_time == -1) { return; }
        if (m_time != 0)
        {
            if (m_gamepad != null) 
            {
            m_gamepad.SetMotorSpeeds(m_power * m_powerController*0.15f, m_power* m_powerController);
            }
            
        }
        m_time--;
        if (m_time < 0)
        {
            if (m_gamepad != null)
            {
                m_gamepad.SetMotorSpeeds(0, 0);
            }
            m_time = -1;
        }
    }
    public float m_time = 0;
    public float m_power = 0;
    public float m_powerController = 1;
    public void setHaptics(float _power, float _time) 
    {
        m_power = _power;
        m_time = _time;
    }
  
}
