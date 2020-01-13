﻿using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {

        Ray lastRay;
        NavMeshAgent navMeshAgent;
        Health health;

        private void Start() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead(); //Para que los enemigos dejen de moverse cuando muere el jugador.
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination) {
            GetComponent<ActionScheduler>().StartAction(this);            
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination;            
            navMeshAgent.isStopped = false;
        }

        //Implemento método de la Interface IAction
        public void Cancel() {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); //Convierte de Global a Local
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}