using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsDetector : MonoBehaviour
{
    public LayerMask grabbable;
    public Text score;
    AudioSource m_audioSource;
    public AudioClip pointsAudio;
    public AudioClip Winner;
    int points = 0;
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1<<other.gameObject.layer) & grabbable) == 1 << other.gameObject.layer)
        {
            points += 100;
            score.text = "" + points;
            Destroy(other.gameObject);
            m_audioSource.PlayOneShot(pointsAudio);
        }
        if(points == 500)
        {
            m_audioSource.PlayOneShot(Winner);

        }
    }
}
