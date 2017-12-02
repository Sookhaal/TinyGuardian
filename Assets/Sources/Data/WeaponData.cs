using Components;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData", order = 0)]
	public class WeaponData : ScriptableObject {
		public int MaxBullets;
		public Bullet BulletPrefab;
		public float BulletVelocity;
	}
}