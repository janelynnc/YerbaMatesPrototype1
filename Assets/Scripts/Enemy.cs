using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour {
    private Animator MovementState;
    private Transform PlayerTarget;
    public bool AttackLocked;
    public float FadeRate;
    public string EnemyLeave;
    public GameObject textbox;
    private AudioSource walking;
    // private Vector3 direction;

    [SerializeField]
    private float StoppingDistance;

    [SerializeField]
    private float MovementSpeed;

    [SerializeField]
    private float MaxSpeed;

    // Use this for initialization
    void Start () {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        MovementState = GetComponent<Animator>();
        walking = GetComponent<AudioSource>();
        AttackLocked = false;
    }

    // Update is called once per frame
    void Update () {
        //EnemyMovement();
        EnemyFollow();
        // Vector3 direction = gameObject.transform.position - PlayerTarget.position;
        // direction = direction.normalized; // find the direction we're going

    }

    public void OnDisable()
    {
        //StartCoroutine("EnemyAttack");
        
    }

    IEnumerator EnemyAttack()
    {
        textbox.SetActive(true);
        GameObject.FindGameObjectWithTag("textboxtext").GetComponent<Text>().text = EnemyLeave;
        Time.timeScale = 0;
        yield return null;

    }
    public void EnemyMovement(float angle)
    {
     
        //print(angle);
        MovementState.SetFloat("z", angle);
        MovementState.SetLayerWeight(1, 1);

    }
    
    public void EnemyFollow()
    {
        if (Vector2.Distance(transform.position, PlayerTarget.position) > StoppingDistance) //if we're further than stopping distance
        {
            if (!walking.isPlaying)
            {
                walking.Play();
            }

            MovementSpeed = MaxSpeed;
            Vector3 direction = PlayerTarget.position - transform.position;

            /*if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y) && Mathf.Abs(direction.x) > Mathf.Epsilon)
            {
                direction.y = 0f;
            }
            else if (Mathf.Abs(direction.y) > Mathf.Epsilon)
            {
                direction.x = 0f;
            }*/

           

            if (Mathf.Abs(direction.x) > .01f)
            {

                if (Mathf.Abs(direction.y) > .01f)
                {
                    direction.y = 0;
                }
                else
                {
                    direction.x = 0;
                }
            }

            if (direction.y > .75f) //up
            {
                EnemyMovement(270);

            }
            else if (direction.y < -.75) //down
            {
                EnemyMovement(90);

            }
            else if (direction.x > 0) //left
            {
                EnemyMovement(180);

            }
            else if (direction.x < 0)//right
            {
                EnemyMovement(0);

            }
            //gameObject.transform.position = Vector2.MoveTowards(transform.position, PlayerTarget.position, MovementSpeed * Time.deltaTime);
            gameObject.transform.position = Vector2.MoveTowards(transform.position, transform.position + direction, MovementSpeed * Time.deltaTime);
            
            MovementState.SetLayerWeight(0, 1);
            MovementState.SetLayerWeight(1, 1);
            MovementState.SetLayerWeight(2, 0);
            
        }
        else
        {
            // attack player


            /* MovementState.SetLayerWeight(0, 0);
             MovementState.SetLayerWeight(1, 0);
             MovementState.SetLayerWeight(2, 1);
             */
            if (!walking.isPlaying)
            {
                walking.Stop();
            }
            if (!AttackLocked)
            {
                StartCoroutine("Attack");
            }
           
            
        }
    }

    IEnumerator Attack()
    {
        AttackLocked = true;
        print("attacking");
        MovementState.SetLayerWeight(0, 0);
        MovementState.SetLayerWeight(1, 0);
        MovementState.SetLayerWeight(2, 1);
        PlayerTarget.gameObject.SendMessage("takedamage");
        yield return new WaitForSeconds(0.5f);
        
        MovementState.SetLayerWeight(0, 0);
        MovementState.SetLayerWeight(1, 0);
        MovementState.SetLayerWeight(2, 0);
        yield return new WaitForSeconds(1f);
        print("idle");
        AttackLocked = false;
        yield return null;


    }

   
}
