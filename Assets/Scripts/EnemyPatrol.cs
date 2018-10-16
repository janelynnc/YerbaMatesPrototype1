using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Animator MovementState;
    private Transform PlayerTarget;
    private Vector2 MovementDirection;

    [SerializeField]
    private float MovementSpeed;
    private float StartTime;
    private float TotalTime;
    public float idleTime;
    public bool follow;
    public List<Transform> targets;
    private int i = 0;
    public float proximity;
    // Use this for initialization
    void Start()
    {
       
        //targets = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        MovementState = GetComponent<Animator>();
        EnemyMovement(targets[i].transform.eulerAngles.z);
    }
    private void OnEnable()
    {
   
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(transform.position,targets[i].position)> proximity ) //if we havent reached our target
        {
            Vector3 direction = targets[i].position - transform.position;
           

            // Vector2 direction = gameObject.transform.position - targets[i].position; //find which direction we're going
            //direction = direction.normalized;

            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y) && Mathf.Abs(direction.x) > Mathf.Epsilon)
            {
                direction.y = 0f;
            }
            else if (Mathf.Abs(direction.y) > Mathf.Epsilon)
            {
                direction.x = 0f;
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
            
            
            //gameObject.transform.position = Vector2.MoveTowards(transform.position, targets[i].position, MovementSpeed * Time.deltaTime);
            gameObject.transform.position = Vector2.MoveTowards(transform.position, transform.position+direction, MovementSpeed * Time.deltaTime);

            //Call EnemyMovement with targets[i].transform.rotation.z

        }
        else 
        {
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
        }
        
        
    }

    public void EnemyMovement(float angle)
    {

        print(angle);
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
