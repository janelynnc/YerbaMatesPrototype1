using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float StartX;
    private Animator MovementState;

    [SerializeField]
    private float MovementSpeed;

    public List<Transform> targets;
    private int i = 0;
    public float proximity;
    // Use this for initialization
    void Start () {
        MovementState = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        //EnemyMovement();
        if(Vector3.Distance(gameObject.transform.position,targets[i].position)>= proximity)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targets[i].position, Time.deltaTime * MovementSpeed);
            //Call EnemyMovement with targets[i].transform.rotation.z
            EnemyMovement(targets[i].transform.eulerAngles.z);
        }
        else
        {
            //play idle animation here
  
            if (i == targets.Count - 1) //If we reached the last target
            {
                i = 0; //set our index to 0
            }
            else
            {
                i++; // move on to the next index
            }
        }
    }

    public void EnemyMovement(float angle)
    {
     
        print(angle);
        MovementState.SetFloat("z", angle);
        MovementState.SetLayerWeight(1, 1);

    }
}
