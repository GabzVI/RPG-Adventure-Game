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
	[SerializeField] Health health;
	[SerializeField] Canvas rootCanvas;
	GameObject player;

	private void Awake()
	{
		slider.minValue = 0f;
	}

	// Update is called once per frame
	void Update()
	{ 
		slider.value = health.GetHealthPercentage();

		if (slider.value == 1)
		{
			rootCanvas.enabled = false;
		}
		else
		{
			rootCanvas.enabled = true;
		}

		if (slider.value == 0)
		{
			canvasGroup.alpha = 0;
			rootCanvas.enabled = false;
			return;
		}
		
		//rootCanvas.enabled = true;
		
	}
}
