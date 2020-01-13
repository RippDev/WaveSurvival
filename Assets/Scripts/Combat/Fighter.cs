using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime; //Cuenta el paso de los segundos para medir tiempo

            if (target == null) return; //Si el objetivo no existe, salimos
            if (target.IsDead()) return; //Si el objetivo está muerto, salimos
            
            //Comportamiento de ataque
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                //Esto dispara el evento Hit()
                TriggerAttack();
                timeSinceLastAttack = 0f; //Reseteamos el contador de tiempo para que cada ataque se haga cada X segundos                
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack"); //Desactiva el trigger de StopAttack por si estaba activado de antes
            GetComponent<Animator>().SetTrigger("attack"); //Cuando paramos de movernos, atacamos
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        /* La animación de ataque sin armas tiene un evento en el cuadro 11, que es justo cuando el puño del personaje golpea al objetivo. En este evento, se puede 
        * disparar un efecto de partículas, un indicador de daño, etc. A continuación vamos a crear el método Hit() que se ejecutará al golpear al objetivo
        */
        //The following is an animation event
        void Hit()
        {
            //Ejecutando este código en este método hacemos que el daño sea aplicado cuando la animación alcanza el cuadro 11, en el momento en el que el puño golpea al objetivo            
            if (target == null) return;
            target.TakeDamage(weaponDamage); //Causamos daño al oponente
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        //Implemento método de la Interface IAction
        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}