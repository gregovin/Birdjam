using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsDetector : MonoBehaviour
{
    public LayerMask grabbable;
    public Text score;
    AudioSource m_audioSource;
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1<<other.gameObject.layer) & grabbable) == 1 << other.gameObject.layer)
        {
            score.text = "" + (int.Parse(score.text) + 100);
            Destroy(other.gameObject);
            m_audioSource.Play();
        }
    }
}
