using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;
using TMPro;

namespace RPG.Combat
{
	public class EnemyHealthDisplay: MonoBehaviour
	{
		[SerializeField] TMP_Text textPro;
		FighterScript fighter;
		

		private void Awake()
		{
			fighter = GameObject.FindWithTag("Player").GetComponent<FighterScript>();
		}

		// Update is called once per frame
		void Update()
		{
			if(fighter.GetTarget() == null) { textPro.text = "N/A"; return; }
			Health health = fighter.GetTarget();

			textPro.text = string.Format("{0:0} / {1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
			
		
		}
	}
}

