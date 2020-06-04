using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalePersonScript : MonoBehaviour
{
    public Collider2D[] colliders;
    public Collider2D attackBlob;
    public AudioClip walkingAudio;
    public AudioClip attackingAudio;
    float timeKeeper = 0;
    int colliderChoosen = 0;
    int dir = 1;
    bool facing = false;
    public Transform defaultPos;
    private Rigidbody2D m_Rigidbody;
    private Transform m_Transform;
    private AudioSource m_AudioSource;
    bool walking = false;
    GameObject target;
    Animator controller;
    // Start is called before the first frame update
    void Start()
    {
        colliders[colliderChoosen].enabled = false;
        controller = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Transform = GetComponent<Transform>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.SetBool("LookingUp", facing);
    }
    void FixedUpdate()
    {
        if (!walking)
        {
            timeKeeper += Time.fixedDeltaTime;
            if (timeKeeper > 5)
            {
                colliders[colliderChoosen].enabled = true;
                colliderChoosen = (colliderChoosen + 1) % colliders.Length;
                colliders[colliderChoosen].enabled = false;
                timeKeeper = 0;
                facing = !facing;

            }
        }
        else
        {
            Vector3 relativeDisplacement = target.transform.position - m_Transform.position;
            float mag = relativeDisplacement.x * relativeDisplacement.x + relativeDisplacement.y * relativeDisplacement.y;
            Vector3 normalDisplacement = relativeDisplacement;
            normalDisplacement.Normalize();
            normalDisplacement.y = 0;
            m_Rigidbody.velocity = normalDisplacement;
            controller.SetBool("Attacking", mag < 4);
            attackBlob.enabled = mag < 4;
            if (normalDisplacement.x * dir > 0)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                dir *= -1;
                transform.localScale = theScale;
            }
            if (mag > 16)
            {
                Walkback();
            }
            else if (!m_AudioSource.isPlaying && mag > 4)
            {
                m_AudioSource.PlayOneShot(walkingAudio);

            }
            else if (mag < 4)
            {
                m_AudioSource.PlayOneShot(attackingAudio);
            }
        }
    }
    public void Walkback()
    {
        Vector3 relativeDisplacement = defaultPos.position - m_Transform.position;
        float mag2 = relativeDisplacement.x * relativeDisplacement.x + relativeDisplacement.y * relativeDisplacement.y;
        Vector3 normalDisplacement = relativeDisplacement;
        normalDisplacement.Normalize();
        normalDisplacement.y = 0;
        if (normalDisplacement.x * dir > 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            dir *= -1;
            transform.localScale = theScale;
        }
        m_Rigidbody.velocity = normalDisplacement;
        if(mag2 < 0.2)
        {
            walking = false;
            controller.SetBool("Walking", false);
            if(dir != 1)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                dir *= -1;
                transform.localScale = theScale;
            }
        }
    }
    public void Seen(GameObject other)
    {
        walking = true;
        controller.SetBool("Walking", true);
        target = other;
    }
}
