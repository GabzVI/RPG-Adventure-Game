using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
	public class ExperienceDisplay : MonoBehaviour
	{
		[SerializeField] TMP_Text textPro;
		Experience experience;
		BaseStats baseStats;

		private void Awake()
		{
			experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
			baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();

		}

		// Update is called once per frame
		void Update()
		{
			textPro.text = string.Format("{0:0} / {1:0}", experience.GetExperiencePoints(), baseStats.GetMaxExpToLevelUp());
		}
	}
}


