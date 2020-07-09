using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;
namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }
        // Update is called once per frame
        void Update()
        {
            //If player is dead will disable the raycast component and movement.
            if (health.IsDead()) { return; }
            if (InteractWithCombat()) { return; }
            if (InteractWithMovement()) { return; }
        }

        private bool InteractWithCombat()
        {
            //This is done so that the ray penetrates any object and targets/detects enemies as a priority.
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            //This will check for all the objects the ray cast hit and store it inside hitInfo.
            foreach (RaycastHit hitInfo in hits)
            {
                CombatTarget target = hitInfo.transform.GetComponent<CombatTarget>();
                if (target == null) { continue; }

                //Continue is to continue loop and check for other objects until target isnt null is found.
                if (!GetComponent<FighterScript>().CanAttack(target.gameObject))
                {
                    continue;
                }
                  
                if (Input.GetMouseButton(1))
                {
                    GetComponent<FighterScript>().Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }


        private bool InteractWithMovement()
        {
            RaycastHit hitInfo;

            //Stores the ray thats being shot from camera and stores whats it hitting into hitInfo.
            bool hasHit = Physics.Raycast(GetMouseRay(), out hitInfo);
            if (hasHit)
            {
                if (Input.GetMouseButton(1))
                {
                    //1f is to ensure player moves at full speed
                    GetComponent<Mover>().StartMoving(hitInfo.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

