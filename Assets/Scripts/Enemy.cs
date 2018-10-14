using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private Vector2 MovementDirection;
    public float StartX;
    private Animator MovementState;
    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private float MaxSteps;

    // Use this for initialization
    void Start () {
        StartX = transform.position.x;
    }

    // Update is called once per frame
    void Update () {
        EnemyMovement();
    }

    public void EnemyMovement()
    {
        transform.Translate(MovementSpeed * Time.deltaTime, 0,0);
        if(Mathf.Abs(StartX - transform.position.x) > MaxSteps)
        {
            MovementSpeed *= -1.0f;
        }
        

        
    }
}
