﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
			textPro.text = String.Format("{0:0} / {1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
		}
	}
}

