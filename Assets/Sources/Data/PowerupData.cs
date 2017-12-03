using UnityEngine;
using UnityEngine.Events;

namespace Data {
	[CreateAssetMenu(fileName = "Powerup", menuName = "Data/PowerupData", order = 0)]
	public class PowerupData : ScriptableObject {
		public UnityEvent WhatHappens;
	}
}