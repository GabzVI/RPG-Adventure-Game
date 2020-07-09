using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
	public class Fader : MonoBehaviour
	{
		CanvasGroup canvasGroup;

		private void Start()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public void FadeOutQuick()
		{
			canvasGroup.alpha = 1;
		}
		public IEnumerator FadeOut(float time)
		{
			while(canvasGroup.alpha < 1)//Do while alpha isnt 1.
			{
				canvasGroup.alpha += Time.deltaTime / time;
				yield return null;
			}
		}

		public IEnumerator FadeIn(float time)
		{
			while (canvasGroup.alpha > 0)//Do while alpha is bigger than 0.
			{
				canvasGroup.alpha -= Time.deltaTime / time;
				yield return null;
			}
		}
	}
}

