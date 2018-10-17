using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D RB;
    private Animator MovementState;
    private Vector2 MovementDirection;
    public float angle;
    public float PlayerHealth;
    public List<GameObject> PlayerHearts;
    public Enemy IsAttacking;
    public bool HealthLocked = false;
 
    [SerializeField]
    private float MovementSpeed; // set value in inspector

	// Use this for initialization
	void Start () {
        PlayerHealth = PlayerHearts.Count;
        RB = GetComponent<Rigidbody2D>();
        MovementState = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update () {
        //print("Hearts Left = " + PlayerHealth);
        GetKeyInput();
        PlayerMovement();
        
    }
 
    // Player movement function
    public void PlayerMovement()
    {
        // Lets player move
        //print(MovementDirection);
        transform.Translate(MovementDirection * MovementSpeed * Time.deltaTime);
        //RB.velocity = MovementDirection * MovementSpeed;
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
    // Conditons and values of x and y are assigned in the animator
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
        //print(Input.anyKey);
        // Move up
        if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)))
        {
            MovementDirection += Vector2.up;
            
        }

        // Move left
        else if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
        {
            MovementDirection += Vector2.left;
        }

        // Move down
        else if ((Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)))
        {
            MovementDirection += Vector2.down;
        }

        // Move right
        else if ( (Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)) )
        {
            MovementDirection += Vector2.right;
        }
        //print(MovementDirection);
    }

    // Collision - checking
    private void OnCollisionEnter2D(Collision2D collision)
    {

        print("collide");
        GameObject PlayerCollidesWith = collision.gameObject;
        if(PlayerCollidesWith.tag == "Water")
        {
            GameObject Map = GameObject.FindGameObjectWithTag("Map");
            if (Map != null)
            {

                GameObject.Destroy(Map);
            }
        }
        if(PlayerCollidesWith.tag == "Enemy")
        {
            
           // GameObject Enemy = PlayerCollidesWith.gameObject;
            //if (Enemy.GetComponent<Enemy>().AttackLocked)
          //  {
                /*
                
                if (!HealthLocked)
                {
                    print("lose");
                    //StartCoroutine(LoseHealth(Enemy));
                    //StopCoroutine("LoseHealth");
                }
                */
               if (PlayerHealth > 0)
                {

                //StartCoroutine("KnockBack");
                //RB.AddForce(transform.forward * -6);
                // PlayerHealth--;
                //GameObject.Destroy(PlayerHearts[PlayerHearts.Count - 1]);
                //PlayerHearts.RemoveAt(PlayerHearts.Count - 1);
                }
          //  }
            
        }
    }

    public bool isAttacking(GameObject Enemy)
    {
        return Enemy.GetComponent<Enemy>().AttackLocked;
    }
    IEnumerator KnockBack()
    {
        /*
        yield return new WaitUntil(() => Enemy.GetComponent<Enemy>().AttackLocked == false);
        if (PlayerHealth > 0)
        {
            PlayerHealth--;
            GameObject.Destroy(PlayerHearts[PlayerHearts.Count - 1]);
            PlayerHearts.RemoveAt(PlayerHearts.Count - 1);
        }
        */
        yield return new WaitForSeconds(1f);
        RB.AddForce(transform.forward * -6);
        PlayerHealth--;
        GameObject.Destroy(PlayerHearts[PlayerHearts.Count - 1]);
        PlayerHearts.RemoveAt(PlayerHearts.Count - 1);
        //HealthLocked = true;

        yield return null;
        // HealthLocked = false;


    }

    public void takedamage()
    {
        if (PlayerHealth > 0)
        {
            PlayerHealth--;
            GameObject.Destroy(PlayerHearts[PlayerHearts.Count - 1]);
            PlayerHearts.RemoveAt(PlayerHearts.Count - 1);
        }

    }
}
