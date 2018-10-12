using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D RB;
    private Animator MovementState;
    private Vector2 MovementDirection;
    
    [SerializeField]
    private float MovementSpeed; // set value in inspector

	// Use this for initialization
	void Start () {
        RB = GetComponent<Rigidbody2D>();
        MovementState = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update () {
        GetKeyInput();
        PlayerMovement();
    }
 
    public void PlayerMovement()
    {
        // Lets player move
        transform.Translate(MovementDirection * MovementSpeed * Time.deltaTime);
        
        // If the player is moving, play walking animation
        // Otherwise, play idle animation
        if(MovementDirection.x != 0 || MovementDirection.y != 0)
        {
            AnimatedMovement(MovementDirection);
        }
        else
        {
            MovementState.SetLayerWeight(1, 0);
        }
    }

    // Walking animation for the player
    // Conditons are assigned in the animator
    public void AnimatedMovement(Vector2 MovementDirection)
    {
        MovementState.SetFloat("x", MovementDirection.x);
        MovementState.SetFloat("y", MovementDirection.y);
        MovementState.SetLayerWeight(1, 1);
    }

    // Assign keys here
    private void GetKeyInput()
    {
        MovementDirection = Vector2.zero;

        // Move up
        if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)))
        {
            MovementDirection += Vector2.up;

        }

        // Move left
        if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
        {
            MovementDirection += Vector2.left;

        }

        // Move down
        if ((Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)))
        {
            MovementDirection += Vector2.down;

        }

        // Move right
        if ( (Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)) )
        {
            MovementDirection += Vector2.right;

        }
    }
}
