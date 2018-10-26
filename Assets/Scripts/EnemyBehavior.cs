using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyBehavior : MonoBehaviour {
    public Transform PlayerPos;
    public float MinDistance;
    public Player PlayerHealth;
    public MonoBehaviour patrol;
    public MonoBehaviour follow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //Get the players transform
        if (Vector2.Distance(transform.position, PlayerPos.position) <= MinDistance) //if the we're closer to the player than mindistance
        {
            //gameObject -> get the gameobject we're attached to
            //.GetComponent<>() -> get the script
            // enabled sets script on/off

            
            if (PlayerHealth.PlayerHealth == 0)
            {
                patrol.enabled = true; //stop patrolling
                follow.enabled = false;
                
            }
            else
            {
                patrol.enabled = false; //stop patrolling
                follow.enabled = true; //start following
            }
        }


        
       
		
	}
   
}
