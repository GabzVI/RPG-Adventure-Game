using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
    
namespace RPG.Combat
{
    public class FighterScript : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1.0f;
        [SerializeField] float weaponDamage = 5f;
        Health target;

        //Will enable characters to attack straight away.
        float timeForLastAttack = Mathf.Infinity;

        private void Update()
        {
            timeForLastAttack += Time.deltaTime;

            if (target == null) { return; }
            if (target.IsDead()) { return; }

            if (!GetIsinRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            
            if (timeForLastAttack > timeBetweenAttacks)
            {
                //This will rotate the character towards the enemy that it will hit.
                transform.LookAt(target.transform);

                TriggetAttack();
                timeForLastAttack = 0f;

            }

        }

        private void TriggetAttack()
        {
            //This will reset the stopAttack trigger to false.
            GetComponent<Animator>().ResetTrigger("stopAttack");
            //This will trigger the Hit() event on animation to deal damage.
            GetComponent<Animator>().SetTrigger("Attack");
        }

        //Animation Event 
        void Hit()
        {
            if(target == null) { return; }
           target.TakeDamage(weaponDamage);
        }

        private bool GetIsinRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();

        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
       
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

    }
}

