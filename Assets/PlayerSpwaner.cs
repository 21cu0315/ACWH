using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpwaner : MonoBehaviour
{
    [SerializeField] GameObject Player;
    void SpwanPlayer() 
    {
        Instantiate(Player, transform.position, Quaternion.identity);
        Destroy(this);
        Destroy(gameObject, 2f);
        FindObjectOfType<SoundManager>().PlayAudioOneShot("Enemy_Spwan");
        FindObjectOfType<GameRuler>().SetPlayer();
    }
}
