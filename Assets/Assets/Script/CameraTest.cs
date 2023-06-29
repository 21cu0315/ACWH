using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public GameObject m_target;
    Camera m_camera;
    public bool m_slowFollow;
    private void Start()
    {
        m_camera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        //if (!m_slowFollow)
        //{
            var _tarPos = new Vector3(m_target.transform.position.x, m_target.transform.position.y + 1.5f, m_target.transform.position.z);
            var _pos = Vector2.Lerp(transform.position, _tarPos, Time.deltaTime * 5f);
            transform.position = new Vector3(_pos.x, _pos.y, -10f);
            if (Input.GetKeyDown(KeyCode.JoystickButton9))
            {
                m_camera.orthographic = !m_camera.orthographic;
            }
        //}
        //else 
        //{
            //var _tarPos = new Vector3(m_target.transform.position.x, m_target.transform.position.y + 1.5f, m_target.transform.position.z);
            //var _pos = Vector2.Lerp(transform.position, _tarPos, Time.deltaTime * 3f);
            //transform.position = new Vector3(_pos.x, _pos.y, -10f);
            //if (Input.GetKeyDown(KeyCode.JoystickButton9))
            //{
            //    m_camera.orthographic = !m_camera.orthographic;
            //}
        //}
    }
}
