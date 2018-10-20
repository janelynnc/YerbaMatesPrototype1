using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
public class Player : MonoBehaviour {

    public List<TileBase> treetiles;
    private int TileArrayCounter = 0;
    public List<Tilemap> tilemaps;

    public List<TileBase> bushtiles;
    public List<TileBase> bush2tiles;
    public List<TileBase> tree2tiles;
    

    private Rigidbody2D RB;
    private Animator MovementState;
    private Vector2 MovementDirection;
    public float angle;
    public float PlayerHealth;
    public List<GameObject> PlayerHearts;
    public Enemy IsAttacking;
    public bool HealthLocked = false;
    public float FadeRate;
    public string EnemyLeave;
    [SerializeField]
    private float MovementSpeed; // set value in inspector
    public GameObject textbox;
    public string maptext;
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
        //transform.Translate(MovementDirection * MovementSpeed * Time.deltaTime);
        RB.velocity = MovementDirection * MovementSpeed * Time.deltaTime;
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
           if(textbox.activeInHierarchy == false && Map != null)
            {
                StartCoroutine("losemap");
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
            StartCoroutine("FadeHeart");
            
        }

    }

    IEnumerator FadeHeart()
    {

        for (float i = 1; i > 0f; i -= .1f)
        {
            PlayerHearts[PlayerHearts.Count - 1].GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return new WaitForSecondsRealtime(FadeRate);
        }
        PlayerHearts.RemoveAt(PlayerHearts.Count - 1);
        if(PlayerHearts.Count == 0)
        {
            if (TileArrayCounter < treetiles.Count - 1)
            {
                if (TileArrayCounter < treetiles.Count - 1)
                {
                    tilemaps[0].SwapTile(treetiles[TileArrayCounter], treetiles[TileArrayCounter + 1]);
                    tilemaps[1].SwapTile(bushtiles[TileArrayCounter], bushtiles[TileArrayCounter + 1]);
                    tilemaps[1].SwapTile(bush2tiles[TileArrayCounter], bush2tiles[TileArrayCounter + 1]);
                    tilemaps[0].SwapTile(tree2tiles[TileArrayCounter], tree2tiles[TileArrayCounter + 1]);
                    TileArrayCounter++;
                }
                TileArrayCounter++;
            }
        }
        yield return null;
    }

    IEnumerator losemap()
    {
        textbox.SetActive(true);
        GameObject.FindGameObjectWithTag("textboxtext").GetComponent<Text>().text = maptext;
        Time.timeScale = 0;
        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        if (Map != null)
        {
            for (float i = 1; i > 0f; i -= .1f)
            {
                Map.GetComponent<Image>().color = new Color(1, 1, 1, i);
                yield return new WaitForSecondsRealtime(FadeRate);
            }
            GameObject.Destroy(Map);
        }
        if (TileArrayCounter < treetiles.Count - 1)
        {
            tilemaps[0].SwapTile(treetiles[TileArrayCounter], treetiles[TileArrayCounter + 1]);
            tilemaps[1].SwapTile(bushtiles[TileArrayCounter], bushtiles[TileArrayCounter + 1]);
            tilemaps[1].SwapTile(bush2tiles[TileArrayCounter], bush2tiles[TileArrayCounter + 1]);
            tilemaps[0].SwapTile(tree2tiles[TileArrayCounter], tree2tiles[TileArrayCounter + 1]);
            TileArrayCounter++;
        }
        yield return null;

    }
}
