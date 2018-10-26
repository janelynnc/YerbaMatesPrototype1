using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Pathfinding {
	public class AIDestinationSetter : VersionedMonoBehaviour {
        /// <summary>The object that the AI should move to</summary>
        private Animator MovementState;
        public Transform target;
        IAstarAI ai;
        public string EnemyLeave;
        public GameObject textbox;
        public bool AttackLocked;
        private AudioSource walking;
        public Vector3 prev_position;

        void OnEnable () {
			ai = GetComponent<IAstarAI>();
			if (ai != null) ai.onSearchPath += Update;
            AttackLocked = false;
            walking = GetComponent<AudioSource>();
            MovementState = GetComponent<Animator>();
            prev_position = ai.position;
        }

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
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


        /// <summary>Updates the AI's destination every frame</summary>
        void Update () {
			if (target != null && ai != null) ai.destination = target.position;
            if (Vector2.Distance(target.position, ai.position) < 2.5f) {
                if (!walking.isPlaying)
                {
                    walking.Stop();
                }
                if (!AttackLocked)
                {
                    StartCoroutine("Attack");
                }
            } else {
                if (!walking.isPlaying)
                {
                    walking.Play();
                }
            }

            Vector3 direction = target.position - ai.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle <= 135 && angle >= 45 ) //up
            {
                EnemyMovement(270);

            }
            else if (angle >= -45 && angle <= 45) //left
            {
                EnemyMovement(180);

            }
            else if (angle <= -135 || angle >= 135) //right
            {
                EnemyMovement(0);

            }
            else//down
            {
                EnemyMovement(90);

            }

        }

        IEnumerator Attack()
        {
            AttackLocked = true;
            print("attacking");
            MovementState.SetLayerWeight(0, 0);
            MovementState.SetLayerWeight(1, 0);
            MovementState.SetLayerWeight(2, 1);
            target.gameObject.SendMessage("takedamage");
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
}
