using UnityEngine;
using UnityEngine.Events;
using System;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
    public Transform m_This;
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
    public float glideangle = 10;
    public float gravity = 10;
    float glideMagX = 0;
    
    float glideMagY = 0;
    int direction = 1;
    [Header("Events")]
    [Space]
    GameObject grabing;

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
        glideMagX = (float)(gravity * 6 * Math.Sin(glideangle * Math.PI / 180) * Math.Cos(glideangle * Math.PI / 180));
        glideMagY = (float)(gravity/2 * Math.Sin(glideangle * Math.PI / 180) * Math.Sin(glideangle * Math.PI / 180));

        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
        Flip();
		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{
        if(!Physics2D.OverlapCircle(m_GroundCheck.position, k_CeilingRadius, m_WhatIsGround) && !crouch) {
            m_Rigidbody2D.AddForce(new Vector2(glideMagX * direction, -glideMagY));

        }
		// If crouching, check to see if the character can stand up

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
                m_Rigidbody2D.AddForce(new Vector2(0f, -m_JumpForce));
            } 

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

    private GameObject GetGrabbed()
    {
        if (Physics2D.OverlapCircle(m_GroundCheck.position, k_CeilingRadius, m_WhatIsGround))
        {
            GameObject temp = Physics2D.OverlapCircle(m_GroundCheck.position, k_CeilingRadius, m_WhatIsGround).gameObject;
            if (temp.GetComponent<Renderer>().bounds.size.magnitude < 9)
            {
                return temp;
            }
        }
        return null;
    }
    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
        direction *= -1;
	}
    public void grab(bool prev, bool cur)
    {
        if(cur && prev != cur && grabing !=null)
        {
            grabing.GetComponent<Collider2D>().enabled = true;
            grabing = null;
        } else if(cur && prev != cur)
        {
            if (GetGrabbed() != null) grabing = GetGrabbed();
            grabing.GetComponent<Collider2D>().enabled = false;
        }
        if(grabing != null)
        {
            Vector3 newPos = m_This.position;
            newPos.y -= 1;
            grabing.transform.position = newPos;
        }
    }
}
