using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Resources;
using RPG.Stats;
using RPG.Combat;
public class SliderValue : MonoBehaviour
{
	[SerializeField] Slider slider;
	[SerializeField] CanvasGroup canvasGroup;
	GameObject[] enemies;
	GameObject player;
	FighterScript fighter;


	private void Awake()
	{
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		fighter = GameObject.FindWithTag("Player").GetComponent<FighterScript>();
		player = GameObject.Find("Player");
		slider.minValue = 0f;
	}

	// Update is called once per frame
	void Update()
	{ 
		if(fighter.GetTarget() == null) { return; }
		Health health = fighter.GetTarget();
		
		slider.maxValue = (health.GetMaxHealthPoints() / health.GetMaxHealthPoints());

		slider.value = (health.GetHealthPoints() / health.GetMaxHealthPoints());

		if (slider.value == 0)
		{
			canvasGroup.alpha = 0;
		}
		if (fighter.GetTarget().IsDead())
		{
			slider.maxValue = 1;
		}
	}
}
