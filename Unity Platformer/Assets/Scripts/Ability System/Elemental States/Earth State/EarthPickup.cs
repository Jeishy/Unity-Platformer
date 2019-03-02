﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthPickup : MonoBehaviour {

	private AbilityManager _abilityManager;
	void Start () {
		_abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
	}
	
	// If wind pickup interacts with player,
	// set players elemental state to Wind
	// and run method for running OnWindState event
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			_abilityManager.EarthState();
			Destroy(gameObject);
		}
	}
}
