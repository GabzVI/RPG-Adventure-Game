using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.Resources
{
	public class HealthDisplay : MonoBehaviour
	{
		[SerializeField] TMP_Text textPro;
		Health health;

		private void Awake()
		{
			health = GameObject.FindWithTag("Player").GetComponent<Health>();

		}

		// Update is called once per frame
		void Update()
		{
			textPro.text = string.Format("{0:0}%", health.GetHealthPercentage());
		}
	}
}

