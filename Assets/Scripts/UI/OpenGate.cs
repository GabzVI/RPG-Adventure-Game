using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Cinematics;
using UnityEngine.AI;
using System;


public class OpenGate : MonoBehaviour
{
	[SerializeField] GameObject gate;
	[SerializeField] float moveUpwardsSpeed = 1f;
	Vector3 newGatePos;
	float timeUntilGateStopOpening = 0f;
	NavMeshSurface surface;
	
	private void Start()
	{
		surface = FindObjectOfType<NavMeshSurface>();
		
	}
	// Update is called once per frame
	void Update()
	{
		
		newGatePos = gate.transform.position;
		if (gameObject.GetComponent<CinematicTrigger>().alreadyTriggered)
		{
			timeUntilGateStopOpening += Time.deltaTime;
			if (timeUntilGateStopOpening < 0.25)
			{
				newGatePos.y += moveUpwardsSpeed * Time.deltaTime;
				gate.transform.position = newGatePos;
			}
		
		}
	}

	private void MoveGate()
	{
		newGatePos.y += moveUpwardsSpeed * Time.deltaTime;
		gate.transform.position = newGatePos;
		
	}
}
