using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Animator MovementState;
    private Transform PlayerTarget;

    [SerializeField]
    private float StoppingDistance;

    [SerializeField]
    private float MovementSpeed;

    // Use this for initialization
    void Start () {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        MovementState = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        //EnemyMovement();
        EnemyFollow();
        Vector3 direction = gameObject.transform.position - PlayerTarget.position;
        direction = direction.normalized; // find the direction we're going
        if (direction.y > .75f)//up
        {
            EnemyMovement(90);
        }
        else if(direction.y < -.75)//down
        {
            EnemyMovement(270);
        }else if (direction.x > 0)//left
        {
            EnemyMovement(0);
        }
        else //right
        {
            EnemyMovement(180);
        }
        
       
    }

    public void EnemyMovement(float angle)
    {
     
        print(angle);
        MovementState.SetFloat("z", angle);
        MovementState.SetLayerWeight(1, 1);

    }
    
    public void EnemyFollow()
    {
        if (Vector2.Distance(transform.position, PlayerTarget.position) > StoppingDistance) //if we're further than stopping distance
        {
            gameObject.transform.position = Vector2.MoveTowards(transform.position, PlayerTarget.position, MovementSpeed * Time.deltaTime);
            print(gameObject.transform.position);
            MovementState.SetLayerWeight(0, 1);
            MovementState.SetLayerWeight(1, 1);
            MovementState.SetLayerWeight(2, 0);
        }
        else
        {
            // attack player
            print("attacking");
            MovementState.SetLayerWeight(0, 0);
            MovementState.SetLayerWeight(1, 0);
            MovementState.SetLayerWeight(2, 1);
        }
    }
}
