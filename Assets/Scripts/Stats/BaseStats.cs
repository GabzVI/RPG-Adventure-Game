using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
	public class BaseStats : MonoBehaviour
	{
		[Range(1, 99)]
		[SerializeField] int startingLevel = 1;
		[SerializeField] CharacterClass characterClass;

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}

}
