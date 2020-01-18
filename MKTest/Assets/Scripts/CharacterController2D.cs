using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float jumpForce = 100f;							    //The amount of force added when the player jumps
    [SerializeField] private float jumpTime;                                        //The highest amount of time the player can jump
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	    //How much to smooth out the movement
	[SerializeField] private LayerMask whatIsGround;							    //A mask determining what is ground to the character
	[SerializeField] private Transform groundCheck;						        	//A position marking where to check if the player is grounded

    public AudioSource jumpSound;                                                   //The Audio to be played when the player jumps

    private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();   //Stopwatch to track when/how long the player is in the air
    const float groundRadius = .2f;                                                 //Radius of the overlap circle to determine if grounded
	private bool grounded;                                                          //Whether or not the player is grounded
	private Rigidbody2D rigidBody;                                                  //The character's rigid body
	private Vector3 vel = Vector3.zero;                                             //The character's velocity
    private float jumpCounter;                                                      //The counter for how long the jump has progressed


	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }


	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

	}

    private void FixedUpdate()
    {

        //Start the stopwatch when the player jumps
        if (!grounded)

        {
            sw.Start();
        }

        grounded = false;

        //If a circlecast to the groundcheck position hits anything designated as ground, the player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);


        for (int i = 0; i < colliders.Length; i++)
        {
            //Check if the player is landing on an object
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;

                //If the player has been away from objects for a short time, the player can now be considered landing
                if (sw.ElapsedMilliseconds > (long)60)
                    OnLandEvent.Invoke();
                sw.Reset();
            }
        }
    }


    public void Move(float move, bool jump, bool jumpBoost, bool jumpFinished)
	{

		//Moving the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(move * 10f, rigidBody.velocity.y);
		//Smoothing it out and applying it to the character
		rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref vel, movementSmoothing);

        //If the player should jump
        if (grounded && jump)
        {
            jumpCounter = jumpTime;
            //Add a vertical force to the player
            grounded = false;
            rigidBody.AddForce(new Vector2(0f, jumpForce));
            jumpSound.Play();
        }
        else if (!grounded && jumpBoost)
        {
            //If already in the air, add a smaller force while the key is held
            if (jumpCounter > 0f)
            {
                rigidBody.AddForce(new Vector2(0f, jumpForce / 5));
                jumpCounter -= Time.deltaTime;
            }
            else
            {
                jumpBoost = false;
                jumpCounter = 0f;
            }
        }
    }

    public void UnStuck()
    {
        rigidBody.AddForce(new Vector2(0f, 5f));
    }

    public bool MovementCheck()
    {
        float speed = rigidBody.velocity.magnitude;
        if(speed < 0.5)
        {
            return true;
        }
        return false;
    }
}
