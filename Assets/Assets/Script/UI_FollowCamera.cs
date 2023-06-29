using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FollowCamera : MonoBehaviour
{
    public Transform m_target;
    public Vector3 offset;
    [SerializeField] bool m_fixedPos ;

    [SerializeField] Camera m_cam;
    // Start is called before the first frame update
    void Start()
    {
        m_cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_target) 
        {
            Vector3 _pos = m_cam.WorldToScreenPoint(m_target.position + offset);
            if (transform.position != _pos)
            {
                if (m_fixedPos)
                {
                    transform.position = _pos;
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, _pos, Time.deltaTime * 6f);
                }
            }
        }
       
    }
}
