using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class PlayerMoveWithJump : MonoBehaviour
{
    public float runSpeed = 5;
    public float walkSpeed = 3;
    public float currentSpeed;
    public bool isRunning;
    public float gravity = 12;    
    public Vector3 velocity;
    public float turnSpeed = 100;
    public Text countText;
    public Text winText;
    public float turnSmoothTime = 0.1f;
    public AudioSource keyPickup; 
	public AudioSource doorVictory;
	public float jump = 3f;

    private CharacterController controller;
    private float verticalVelocity;       
    private int count;
    private float turnSmoothVelocity;
    
	Animator animator;
    
    void Start() {
      
		animator = GetComponent<Animator> ();

        controller = GetComponent<CharacterController>();
        count = 0;
        SetCountText ();
        if (count >=1)
        {
            winText.text = "";
        }
            winText.text = "Find all 3 keys to open the door to the house! Use W to move forwards, S to move backwards and A/D to turn left/right.";
		AudioSource[] audio = GetComponents<AudioSource> ();
		keyPickup = audio [0];
		doorVictory = audio [1];
		  
	
    }

    void Update()
    {
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		var v3 = new Vector3 (0, Input.GetAxis ("Horizontal"), 0.0f);
        transform.Rotate (v3 * turnSpeed * Time.deltaTime);
		
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        currentSpeed = ((isRunning ? runSpeed : walkSpeed) * input.magnitude);
		velocity = input.y* transform.forward * currentSpeed;

		

		if (controller.isGrounded)
		{
			verticalVelocity = -gravity*Time.deltaTime;
			if(Input.GetKeyDown(KeyCode.Space))
			{
				verticalVelocity = jump;
			}
		}
		else{
			verticalVelocity -=gravity *Time.deltaTime;
		}
	
		float animationspeedPercent = ((isRunning) ? 1 : .5f) * currentSpeed;
		animator.SetFloat ("speedPercent", animationspeedPercent, turnSmoothTime, Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            count = count + 1;            
            Destroy(other.gameObject);
            keyPickup.Play();
            SetCountText ();
        }
        if (other.gameObject.CompareTag("Door"))
        {
            if (count >=3)
            {
                winText.fontSize = 38;
                winText.text = "Game over! Congratulations you win!"; 
				doorVictory.Play();
                
            }
        }
    }

    void SetCountText ()
    {
        countText.text = "Keys: " + count.ToString();
        if (count >= 3)
        {
            winText.text = "You have all the keys! Go to the house!";
        }
    }

    void LateUpdate()
	{
		
		controller.Move(velocity * Time.deltaTime);
		controller.Move(new Vector3(0,verticalVelocity,0) * Time.deltaTime);

	}
}
