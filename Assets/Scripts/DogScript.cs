using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogScript : MonoBehaviour
{
    public Collider2D[] colliders;
    public Collider2D attackBlob;
    public AudioClip walkingAudio;
    public AudioClip attackingAudio;
    float timeKeeper = 0;
    int dir = 1;
    bool facing = false;
    private Rigidbody2D m_Rigidbody;
    private Transform m_Transform;
    private AudioSource m_AudioSource;
    bool walking = false;
    GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Transform = GetComponent<Transform>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!walking)
        {
            timeKeeper += Time.fixedDeltaTime;
            if (timeKeeper > 5)
            {
                timeKeeper = 0;
                facing = !facing;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                dir *= -1;
                transform.localScale = theScale;
            }
        }
        else
        {
            Vector3 relativeDisplacement = target.transform.position - m_Transform.position;
            float mag = relativeDisplacement.x * relativeDisplacement.x + relativeDisplacement.y * relativeDisplacement.y;
            Vector3 normalDisplacement = relativeDisplacement;
            normalDisplacement.Normalize();
            normalDisplacement.y = 0;
            normalDisplacement.x *= 2;
            if (normalDisplacement.x * dir < 0)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                dir *= -1;
                transform.localScale = theScale;
            }
            m_Rigidbody.velocity = normalDisplacement;
            attackBlob.enabled = mag < 3;
            if (mag > 16)
            {
                walking = false;
            }
            if (!m_AudioSource.isPlaying && mag > 3)
            {
                m_AudioSource.PlayOneShot(walkingAudio);
            }
            else if (mag < 3)
            {
                m_AudioSource.PlayOneShot(attackingAudio);
            }
        }
    }
    public void Seen(GameObject other)
    {
        walking = true;
        target = other;
    }
}
