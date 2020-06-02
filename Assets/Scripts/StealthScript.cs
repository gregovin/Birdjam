using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthScript : MonoBehaviour
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
        controller.SetBool("Looking", facing);
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
        } else
        {
            Vector3 relativeDisplacement = target.transform.position - m_Transform.position;
            float mag = relativeDisplacement.x * relativeDisplacement.x + relativeDisplacement.y * relativeDisplacement.y;
            Vector3 normalDisplacement = relativeDisplacement;
            normalDisplacement.Normalize();
            normalDisplacement.y = 0;
            if(normalDisplacement.x * dir < 0)
            {
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                dir *= -1;
                transform.localScale = theScale;
            }
            m_Rigidbody.velocity = normalDisplacement;
            controller.SetBool("Attacking", mag < 4);
            attackBlob.enabled = mag < 4;
            if(mag > 16)
            {
                walking = false;
                controller.SetBool("Spotted", false);
            }
            if (!m_AudioSource.isPlaying && mag > 4)
            {
                m_AudioSource.PlayOneShot(walkingAudio);
            } else if(mag < 4){
                m_AudioSource.PlayOneShot(attackingAudio);
            }
        }
    }
    public void Seen(GameObject other)
    {
        walking = true;
        controller.SetBool("Spotted", true);
        target = other;
    }
}
