using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
	public class Projectiles : MonoBehaviour
	{
		[SerializeField] float projectileSpeed = 1f;
		[SerializeField] bool IsHoming = true;
		[SerializeField] GameObject hitEffect = null;
		[SerializeField] float maxLifeTime = 5f;
		[SerializeField] GameObject[] destroyOnHit = null;
		[SerializeField] float lifeAfterImpact = 0.2f;

		Health target = null;
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

			transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
		}
		public void SetTarget(Health target, float damage)
		{
			this.target = target;
			this.damage = damage;

			Destroy(gameObject, maxLifeTime);
		}

		private Vector3 GetAimPosition()
		{
			CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
			if (targetCapsule == null) { return target.transform.position; }
			return target.transform.position + Vector3.up * (targetCapsule.height / 2.0f);
		}

		private void OnTriggerEnter(Collider other)
		{
			if(other.GetComponent<Health>() != target) { return; }

			if (target.IsDead()) { return; }

			target.TakeDamage(damage);
			projectileSpeed = 0.0f;

			if(hitEffect != null)
			{
				Instantiate(hitEffect, GetAimPosition(), transform.rotation);
			}

			foreach (GameObject toDestroy in destroyOnHit)
			{
				Destroy(toDestroy);
			}
			Destroy(gameObject, lifeAfterImpact);
		}
	}
}
