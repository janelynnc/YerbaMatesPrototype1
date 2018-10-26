using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Audio;
using System.Linq;

public class Player : MonoBehaviour
{

    public List<TileBase> treetiles;
    private int TileArrayCounter = 0;
    public List<Tilemap> tilemaps;
    public Tilemap Grass;
    public Tilemap Water;
    public Tilemap WaterWalkable;
    public Tilemap WaterProp;

    public List<TileBase> bushtiles;
    public List<TileBase> bush2tiles;
    public List<TileBase> tree2tiles;
    public List<AudioClip> clips;
    private AudioSource walking;
    public GameObject map;
    public GameObject bagprefab;
    public string bagtext;

    private Rigidbody2D RB;
    private Animator MovementState;
    private Vector2 MovementDirection;
    public float angle;
    public float PlayerHealth;
    public List<GameObject> PlayerHearts;
    public Enemy IsAttacking;
    public bool HealthLocked = false;
    public float FadeRate;
    public bool HeartLocked = false;
    public int FadeHeartCalls = 0;
    public AudioSource BGKMusic;
    public string EnemyLeave;
    [SerializeField]
    private float MovementSpeed; // set value in inspector
    public GameObject textbox;
    public string maptext;
    public string actionToPerform = null;
    public List<string> KeyPressed;
    public bool lostMap = false;
    // Use this for initialization
    void Start()
    {
        PlayerHealth = PlayerHearts.Count;
        RB = GetComponent<Rigidbody2D>();
        MovementState = GetComponent<Animator>();
        walking = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //print("Hearts Left = " + PlayerHealth);
        GetKeyInput();
        PlayerMovement();
        if (TileArrayCounter >= treetiles.Count - 1)
        {
            BGKMusic.pitch = -1;
        }
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
        if (MovementDirection.x != 0 || MovementDirection.y != 0)
        {
            AnimatedMovement(MovementDirection);
            if (!walking.isPlaying)
            {
                walking.loop = true;
                walking.clip = clips[0];
                walking.Play();
            }
        }
        else
        {
            MovementState.SetLayerWeight(1, 0);
            if (walking.isPlaying)
            {
                walking.Stop();
            }
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
        if (Input.anyKey)
        {
            if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)))
            {
                if (!KeyPressed.Contains("up"))
                {
                    KeyPressed.Add("up");
                }
            }
            else
            {
                if (KeyPressed.Contains("up"))
                {
                    KeyPressed.Remove("up");
                }
            }

            // Move left
            if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
            {
                if (!KeyPressed.Contains("left"))
                {
                    KeyPressed.Add("left");
                }
            }
            else
            {
                if (KeyPressed.Contains("left"))
                {
                    KeyPressed.Remove("left");
                }
            }

            // Move down
            if ((Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)))
            {
                if (!KeyPressed.Contains("down"))
                {
                    KeyPressed.Add("down");
                }
            }
            else
            {
                if (KeyPressed.Contains("down"))
                {
                    KeyPressed.Remove("down");
                }
            }

            // Move right
            if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
            {
                if (!KeyPressed.Contains("right"))
                {
                    KeyPressed.Add("right");
                }
            }
            else
            {
                if (KeyPressed.Contains("right"))
                {
                    KeyPressed.Remove("right");
                }
            }

            if (KeyPressed.Count > 0) {
                actionToPerform = KeyPressed[KeyPressed.Count - 1];
                switch (actionToPerform)
                {
                    case "up":
                        MovementDirection += Vector2.up;
                        break;
                    case "left":
                        MovementDirection += Vector2.left;
                        break;
                    case "down":
                        MovementDirection += Vector2.down;
                        break;
                    case "right":
                        MovementDirection += Vector2.right;
                        break;
                }
            }
        }
        else
        {
            KeyPressed.Clear();
            actionToPerform = null;
        }

        //print(MovementDirection);
    }


    // Collision - checking
    private void OnCollisionEnter2D(Collision2D collision)
    {

        print("collide");
        GameObject PlayerCollidesWith = collision.gameObject;
        if (PlayerCollidesWith.tag == "Water" && lostMap == false)
        {
            lostMap = true;
            GameObject Map = GameObject.FindGameObjectWithTag("Map");
            if (textbox.activeInHierarchy == false && Map != null)
            {
                GameObject minimap = GameObject.Instantiate(map,
                    gameObject.transform.position + new Vector3(MovementDirection.x, MovementDirection.y, 0)
                    , Quaternion.identity);
                StartCoroutine("losemap", minimap);
            }
        }
        if (PlayerCollidesWith.tag == "Enemy")
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

    public void dropBackpack() {
        StartCoroutine("losebag");
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

            if (!HealthLocked)
            {
                StartCoroutine("FadeHeart");
            }
            else
            {
                FadeHeartCalls++;//put a call on the list
            }
        }

    }

    IEnumerator FadeHeart()
    {
        if (HealthLocked)
        {
            FadeHeartCalls++;
            yield return null;
        }
        HealthLocked = true;
        PlayerHealth--;
        walking.clip = clips[1];
        if (!walking.isPlaying)
        {
            walking.Play();
            walking.loop = false;
        }
        for (float i = 1; i > 0f; i -= .1f)
        {

            PlayerHearts[PlayerHearts.Count - 1].GetComponent<Image>().color = new Color(1, 1, 1, i);
            yield return new WaitForSecondsRealtime(FadeRate);
        }
        PlayerHearts.RemoveAt(PlayerHearts.Count - 1);
        if (FadeHeartCalls > 0 && PlayerHearts.Count > 0)
        {
            FadeHeartCalls--;
            //HealthLocked = false;
            yield return StartCoroutine("FadeHeart");
        }
        if (PlayerHearts.Count == 0)
        {
            if (TileArrayCounter < treetiles.Count - 1)
            {
                tilemaps[0].SwapTile(treetiles[TileArrayCounter], treetiles[TileArrayCounter + 1]);
                tilemaps[1].SwapTile(bushtiles[TileArrayCounter], bushtiles[TileArrayCounter + 1]);
                tilemaps[1].SwapTile(bush2tiles[TileArrayCounter], bush2tiles[TileArrayCounter + 1]);
                tilemaps[0].SwapTile(tree2tiles[TileArrayCounter], tree2tiles[TileArrayCounter + 1]);
                TileArrayCounter++;
            }
            textbox.SetActive(true);
            GameObject.FindGameObjectWithTag("textboxtext").GetComponent<Text>().text = EnemyLeave;
            Time.timeScale = 0;
            HealthLocked = false;
            Color magenta = new Vector4(1f, 0f, 1f, 1f);
            Color murky = new Vector4(1f, 1f, 0f, 1f);
            Grass.color = magenta;
            Water.color = murky;
            WaterProp.color = murky;
            WaterWalkable.color = murky;
            yield return null;
        }
        HealthLocked = false;
        yield return null;
    }
    IEnumerator losebag()
    {
        Time.timeScale = 0;
        GameObject backpack = GameObject.FindGameObjectWithTag("backpack");
        if (backpack != null)
        {
            GameObject bag = GameObject.Instantiate(bagprefab,
                    gameObject.transform.position + new Vector3(MovementDirection.x, MovementDirection.y, 0)
                    , Quaternion.identity);
            for (float i = 1; i > 0f; i -= .1f)
            {
                bag.GetComponent<SpriteRenderer>().color =
                    Vector4.Scale(bag.GetComponent<SpriteRenderer>().color,
                    new Vector4(1f, 1f, 1f, .8f));
                backpack.GetComponent<Image>().color = new Color(1, 1, 1, i);
                yield return new WaitForSecondsRealtime(FadeRate);
            }
            GameObject.Destroy(backpack);
            GameObject.Destroy(backpack);
            Color magenta = new Vector4(1f, 0f, 1f, 1f);
            Color murky = new Vector4(1f, 1f, 0f, 1f);
            Grass.color = magenta;
            Water.color = murky;
            WaterProp.color = murky;
            WaterWalkable.color = murky;

        }
        if (TileArrayCounter < treetiles.Count - 1)
        {
            tilemaps[0].SwapTile(treetiles[TileArrayCounter], treetiles[TileArrayCounter + 1]);
            tilemaps[1].SwapTile(bushtiles[TileArrayCounter], bushtiles[TileArrayCounter + 1]);
            tilemaps[1].SwapTile(bush2tiles[TileArrayCounter], bush2tiles[TileArrayCounter + 1]);
            tilemaps[0].SwapTile(tree2tiles[TileArrayCounter], tree2tiles[TileArrayCounter + 1]);
            TileArrayCounter++;
        }
        textbox.SetActive(true);
        GameObject.FindGameObjectWithTag("textboxtext").GetComponent<Text>().text = bagtext;
        yield return null;

    }

    IEnumerator losemap(GameObject minimap)
    {

        Time.timeScale = 0;
        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        if (Map != null)
        {
            for (float i = 1; i > 0f; i -= .1f)
            {
                minimap.GetComponent<SpriteRenderer>().color =
                    Vector4.Scale(minimap.GetComponent<SpriteRenderer>().color,
                    new Vector4(1f, 1f, 1f, .8f));
                Map.GetComponent<Image>().color = new Color(1, 1, 1, i);
                yield return new WaitForSecondsRealtime(FadeRate);
            }
            GameObject.Destroy(minimap);
            GameObject.Destroy(Map);
            Color magenta = new Vector4(1f, 0f, 1f, 1f);
            Color murky = new Vector4(1f, 1f, 0f, 1f);
            Grass.color = magenta;
            Water.color = murky;
            WaterProp.color = murky;
            WaterWalkable.color = murky;
        }
        if (TileArrayCounter < treetiles.Count - 1)
        {
            tilemaps[0].SwapTile(treetiles[TileArrayCounter], treetiles[TileArrayCounter + 1]);
            tilemaps[1].SwapTile(bushtiles[TileArrayCounter], bushtiles[TileArrayCounter + 1]);
            tilemaps[1].SwapTile(bush2tiles[TileArrayCounter], bush2tiles[TileArrayCounter + 1]);
            tilemaps[0].SwapTile(tree2tiles[TileArrayCounter], tree2tiles[TileArrayCounter + 1]);
            TileArrayCounter++;
        }
        textbox.SetActive(true);
        GameObject.FindGameObjectWithTag("textboxtext").GetComponent<Text>().text = maptext;
        yield return null;

    }
}
