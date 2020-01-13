using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        [SerializeField] bool isDead = false;

        public bool IsDead() {
            return isDead;
        }

        public void TakeDamage(float damage) {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            print(healthPoints);
            if(healthPoints == 0 && isDead == false)
            {                
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true; //Marca que estamos muertos
            GetComponent<Animator>().SetTrigger("die"); //Lanza la animación de muerte
            GetComponent<ActionScheduler>().CancelCurrentAction(); //Invoca la cancelación de acciones en el ActionScheduler() para que no se mueva luego de muerto
        }
    }
}