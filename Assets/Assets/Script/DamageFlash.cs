using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] Material m_originMaterial;
    [SerializeField] Material m_flashMaterial;
    [SerializeField] SpriteRenderer m_SR;
    [SerializeField] bool m_getDamge;
    [SerializeField]int m_damageCnt = 4;
    [SerializeField]int m_flashFrame ;

    public void SetDamage(bool _damage)
    {
        m_getDamge = _damage;
    }
    public void Flash()
    {
        if (m_damageCnt % m_flashFrame == 0)
        {
            m_SR.material = m_flashMaterial;

        }
        else
        {
            m_SR.material = m_originMaterial;

        }
        m_damageCnt++;
        m_SR.transform.position = new Vector2(transform.position.x + Mathf.Cos(Time.time * 4f) * 0.15f, transform.position.y - Mathf.Sin(Time.time * 4f) * 0.15f) ;
        if (m_damageCnt == 4)
        {
            m_SR.transform.position = transform.position;
            m_damageCnt = 0;
            m_SR.material = m_originMaterial;
            m_getDamge = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_getDamge) 
        {
            Flash();
        } 
    }
}
