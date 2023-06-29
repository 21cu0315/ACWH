using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_imageOrigin;
    [SerializeField] Color m_color;
    [SerializeField] bool m_onAfterImage = false;
    [SerializeField] bool m_onScaleUp = false;
    [SerializeField] int m_interval = 3;
    [SerializeField]List<SpriteRenderer> m_imageSR;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_onAfterImage)
        {
            if (Time.frameCount % m_interval == 0 && Time.timeScale == 1)
            {
                GameObject tmp_afterImage = new GameObject(name + "AfterImage");
                tmp_afterImage.transform.position = m_imageOrigin.transform.position;
                tmp_afterImage.transform.localScale = new Vector3(m_imageOrigin.transform.localScale.x * gameObject.transform.localScale.x, m_imageOrigin.transform.localScale.y);
                SpriteRenderer tmp_image = tmp_afterImage.AddComponent<SpriteRenderer>();
                tmp_image.sprite = m_imageOrigin.sprite;
                tmp_image.flipX = m_imageOrigin.flipX;
                tmp_image.sortingOrder = m_imageOrigin.sortingOrder - 1;
                tmp_image.sortingLayerID = m_imageOrigin.sortingLayerID;
                tmp_image.transform.position = m_imageOrigin.transform.position;
                tmp_image.color = m_color;
                m_imageSR.Add(tmp_image);
                Destroy(tmp_afterImage, 5f / 6f);
                Destroy(tmp_image, 5f / 6f);
            }
        }


        for (int i = 0; i < m_imageSR.Count; i++)
        {
            if (!m_imageSR[i])
            {
                m_imageSR.Remove(m_imageSR[i]);
            }
            else
            {
                if (Time.timeScale == 1) 
                {
                    m_imageSR[i].color = new Color(m_color.r, m_color.g, m_color.b, m_imageSR[i].color.a - 0.02f);

                    if (m_onScaleUp)
                    {
                        m_imageSR[i].transform.localScale = m_imageSR[i].transform.localScale * 1.025f;
                    }
                }
            }
        }
    }
    public void On(bool _on)
    {
        m_onAfterImage = _on;
    }
    public void CleanUp() 
    {
        foreach (SpriteRenderer _obj in m_imageSR) 
        {
            if (_obj) 
            {
                Destroy(_obj.gameObject);
            }
        }
    } 
}
