using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Resources;
using UnityEngine.Events;

namespace RPG.Combat
{
	public class Projectiles : MonoBehaviour
	{
		[SerializeField] float projectileSpeed = 1f;
		[SerializeField] bool IsHoming = true;
		[SerializeField] GameObject hitEffect = null;
		[SerializeField] float maxLifeTime = 1.5f;
		[SerializeField] GameObject[] destroyOnHit = null;
		[SerializeField] float lifeAfterImpact = 0.2f;
		[SerializeField] UnityEvent onHit;

		Health target = null;
		GameObject instigator = null;
		float damage = 0f;

		private void Start()
		{
		   transform.LookAt(GetAimPosition());
		}
		// Update is called once per frame
		void Update()
		{
			if (target == null) { return; }

			if(IsHoming && !target.IsDead())
			{
				transform.LookAt(GetAimPosition());
			}

			gameObject.transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
		}
		public void SetTarget(Health target, GameObject instigator, float damage)
		{
			this.target = target;
			this.damage = damage;
			this.instigator = instigator;

		}

		private Vector3 GetAimPosition()
		{
			CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
			if (targetCapsule == null) { return target.transform.position; }
			return target.transform.position + Vector3.up * (targetCapsule.height / 2.0f);
		}
		private void OnTriggerEnter(Collider other)
		{
			if(other.tag == "Building") { Destroy(gameObject); return; }
			if (other.GetComponent<Health>() != target) { return; }
			if (target.IsDead()) { return; }
			target.TakeDamage(instigator, damage);
			projectileSpeed = 0.0f;

			onHit.Invoke();
			if (hitEffect != null)
			{
				Instantiate(hitEffect, GetAimPosition(), other.transform.rotation);
			}


			foreach (GameObject toDestroy in destroyOnHit)
			{
				Destroy(toDestroy);
			}

			Destroy(gameObject, lifeAfterImpact);

		}
	}
}
