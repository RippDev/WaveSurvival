using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float waypointDellTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        GameObject player;
        Health health;
        Fighter fighter;
        Vector3 guardPosition;
        Mover mover;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArriveAtdWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Start() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (health.IsDead()) return; //Si el NPC muere, saltea el Update

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                // Si el player está a distancia de ataque, es perseguido y golpeado
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveAtdWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            // Si salimos de la distancia de ataque, se cancela el mismo (Ya no se usa)
            //fighter.Cancel();
            //Vuelve a la posición inicial (Ya no se usa)
            //mover.StartMoveAction(guardPosition);

            //Usamos el siguiente código para que el guardia haga su patrulla a través de los Waypoints
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null) {
                if (AtWaypoint()) {
                    timeSinceArriveAtdWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArriveAtdWaypoint > waypointDellTime)
                mover.StartMoveAction(nextPosition);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position,  GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            //Estado de sospecha y vigilancia
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {            
            // Cálculo de la distancia de ataque
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //El siguiente método se usa para que dibuje los gizmos alrededor de los NPCs
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue; //Define el color del Gizmo
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }    
}

