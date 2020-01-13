using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Este script se adjunta al Player y sirve como ordenador de acciones. Es decir, indica cuando estamos corriendo libremente y cuando estamos en modo de ataque. Si atacamos
* a un enemigo, cancela el modo correr y activa la acción de atacar. Cuando dejamos de atacar, cancela dicha acción y activa el modo de movimiento. 
* Este script es llamado desde Mover.cs dentro de StartMoveAction() para activar movimiento. También es llamado desde Fighter.cs dentro de Attack() para activar el modo de ataque
* Las acciones son canceladas desde este mismo script.
*/

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action) {

            if (currentAction == action) return;

            if(currentAction != null)
                currentAction.Cancel();

            currentAction = action;
        }

        public void CancelCurrentAction() {
            StartAction(null); //Cancela todas las acciones luego de morir
        }
    }
}