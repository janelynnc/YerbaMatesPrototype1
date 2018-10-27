using UnityEngine;
using System.Collections;

namespace Pathfinding {
    /// <summary>
    /// Sets the destination of an AI to the position of a specified object.
    /// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
    /// This component will then make the AI move towards the <see cref="target"/> set on this component.
    ///
    /// See: <see cref="Pathfinding.IAstarAI.destination"/>
    ///
    /// [Open online documentation to see images]
    /// </summary>
    [UniqueComponent(tag = "ai.destination")]
    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
    public class AIDestinationSetter : VersionedMonoBehaviour {
        /// <summary>The object that the AI should move to</summary>
        public Transform target;
        IAstarAI ai;
        public bool AttackLocked;
        private Animator MovementState;
        private AudioSource walking;

        void OnEnable () {
            ai = GetComponent<IAstarAI>();
            AttackLocked = false;
            MovementState = GetComponent<Animator>();
            walking = GetComponent<AudioSource>();
            if (ai != null) ai.onSearchPath += Update;
        }

        void OnDisable () {
            if (ai != null) ai.onSearchPath -= Update;

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

            if (Vector2.Distance(target.position, ai.position) < 2.4f)
            {
                if (!walking.isPlaying)
                {
                    walking.Stop();
                }
                if (!AttackLocked)
                {
                    StartCoroutine("Attack");
                }
            }
            else
            {
                if (!walking.isPlaying)
                {
                    walking.Play();
                }
            }

            Vector3 direction = target.position - ai.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle <= 135 && angle >= 45) //up
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
