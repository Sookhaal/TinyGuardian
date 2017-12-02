using Assets.Sources.Components;
using UnityEngine;

namespace Assets.Sources.Data {
	[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData", order = 0)]
	public class WeaponData : ScriptableObject {
		public BulletType BulletType;
		public int MaxBullets;
		public Bullet BulletPrefab;
		public float BulletVelocity;
	}
}