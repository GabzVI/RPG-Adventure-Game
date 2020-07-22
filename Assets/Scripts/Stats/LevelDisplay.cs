using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RPG.Stats
{
	public class LevelDisplay : MonoBehaviour
	{
		[SerializeField] TMP_Text textPro;
		BaseStats baseStats;

		private void Awake()
		{
			baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
		}

		// Update is called once per frame
		void Update()
		{
			textPro.text = string.Format("{0:0}", baseStats.GetLevel());
		}
	}
}

