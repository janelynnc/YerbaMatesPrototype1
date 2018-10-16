using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Animator MovementState;
    private Transform PlayerTarget;

    public bool AttackLocked;
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

        AttackLocked = false;
    }

    // Update is called once per frame
    void Update () {
        //EnemyMovement();
        EnemyFollow();
        // Vector3 direction = gameObject.transform.position - PlayerTarget.position;
        // direction = direction.normalized; // find the direction we're going

    }

    public void EnemyMovement(float angle)
    {
     
        //print(angle);
        MovementState.SetFloat("z", angle);
        MovementState.SetLayerWeight(1, 1);

    }
    
    public void EnemyFollow()
    {
        //print(Vector2.Distance(transform.position, PlayerTarget.position));
        if (Vector2.Distance(transform.position, PlayerTarget.position) > StoppingDistance) //if we're further than stopping distance
        {
            MovementSpeed = MaxSpeed;
            Vector3 direction = PlayerTarget.position - transform.position;

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
            //gameObject.transform.position = Vector2.MoveTowards(transform.position, PlayerTarget.position, MovementSpeed * Time.deltaTime);
            gameObject.transform.position = Vector2.MoveTowards(transform.position, transform.position + direction, MovementSpeed * Time.deltaTime);
            print(gameObject.transform.position);
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
        yield return new WaitForSeconds(2f);
        MovementState.SetLayerWeight(0, 0);
        MovementState.SetLayerWeight(1, 0);
        MovementState.SetLayerWeight(2, 0);
        yield return new WaitForSeconds(3f);
        print("idle");
        AttackLocked = false;
        yield return null;


    }

   
}
