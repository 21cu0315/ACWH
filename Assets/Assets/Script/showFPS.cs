using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class showFPS : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float fps = (1f / Time.deltaTime);
        text.text = Mathf.Round(fps).ToString();
    }
}
