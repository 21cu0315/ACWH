using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sprite : MonoBehaviour
{
    [SerializeField] GameObject m_playerDeathPrefab;
   void PlayerDeath() 
    {
        var tmp_SR = GetComponent<SpriteRenderer>();
        var tmp_death = Instantiate(m_playerDeathPrefab, tmp_SR.transform.position, tmp_SR.transform.rotation);
        tmp_death.transform.localScale = tmp_SR.transform.lossyScale;
        foreach (SpriteRenderer _sr in tmp_death.GetComponentsInChildren<SpriteRenderer>())
        {
            _sr.sprite = tmp_SR.sprite;
            _sr.sortingOrder = tmp_SR.sortingOrder;
        }
        Destroy(tmp_death, 0.75f);
        Destroy(gameObject);
        FindObjectOfType<UI_Script>().m_sceneEnd = true;

        FindObjectOfType<SoundManager>().PlayAudioOneShot("Enemy_Delete");
    }
}
