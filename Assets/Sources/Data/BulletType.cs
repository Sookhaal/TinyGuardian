using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "BulletType", menuName = "Data/BulletType", order = 0)]
	public class BulletType : ScriptableObject {
		[Tooltip("Will do bonus damage against that type.")]
		public BulletType BonusDamageType;
	}
}