using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData", order = 0)]
	public class EnemyData : ScriptableObject {
		public BulletType EnemyType;
		public float StartingHP = 1f;
		public float TimeBetweenShot = 0.3f;
		public SpreadType ShootingPattern;
		public int SelectedWeapon;
		public float ScoreValue;
		public float BonusScore;
	}
}