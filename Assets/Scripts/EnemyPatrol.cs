using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Animator MovementState;
    private Transform PlayerTarget;
    private Vector2 MovementDirection;
    private AudioSource walking;
    [SerializeField]
    private float MovementSpeed;
    private float StartTime;
    private float TotalTime;
    public float idleTime;
    public bool follow;
    public List<Transform> targets;
    private int i = 0;
    public float proximity;
    public float collisionDistance;
    private GameObject player;
    // Use this for initialization
    void Start()
    {
       
        //targets = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        MovementState = GetComponent<Animator>();
        EnemyMovement(targets[i].transform.eulerAngles.z);
        walking = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnEnable()
    {
   
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(transform.position,targets[i].position)> proximity && Vector2.Distance(player.transform.position,transform.position)>collisionDistance) //if we havent reached our target
        {
            print(Vector2.Distance(player.transform.position, transform.position));
            Vector3 direction = targets[i].position - transform.position;
           // print(direction);
          //  print(direction);

            // Vector2 direction = gameObject.transform.position - targets[i].position; //find which direction we're going
           // direction = direction.normalized;

            /*if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y) && Mathf.Abs(direction.x) > Mathf.Epsilon)
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
                    direction.y = 0f;
                }
                else
                {
                    direction.x = 0f;
                }
            }
            

            if (direction.y > 0) //up
            {
                EnemyMovement(270);
               
            }
            else if (direction.y < 0) //down
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



            gameObject.transform.position = Vector2.MoveTowards(transform.position, transform.position+direction, MovementSpeed * Time.deltaTime);
            //RB.velocity = direction * MovementSpeed * Time.deltaTime;
            if (!walking.isPlaying)
            {
                walking.Play();
            }
            //Call EnemyMovement with targets[i].transform.rotation.z

        }
        else 
        {
            //RB.velocity = Vector3.zero;
            //play idle animation here 
            //StartCoroutine("idle");
            if (i == targets.Count - 1) //If we reached the last target
            {
                i = 0; //set our index to 0
            }
            else
            {
               

                i++; // move on to the next index
            }
            StartTime = Time.time;
            TotalTime = Vector3.Distance(gameObject.transform.position, targets[i].position) / MovementSpeed;
            if (walking.isPlaying)
            {
                walking.Stop();
            }
        }
        
        
    }

  
    public void EnemyMovement(float angle)
    {

        //print(angle);
        MovementState.SetFloat("z", angle);
       // MovementState.SetLayerWeight(0, 0);
        MovementState.SetLayerWeight(1, 1);

    }
    IEnumerator idle()
    {

        MovementState.SetLayerWeight(1, 0);
        //MovementState.Play("EnemyRightIdle");
        yield return new WaitForSeconds(idleTime);
        EnemyMovement(targets[i].transform.eulerAngles.z);

    }

   
}
