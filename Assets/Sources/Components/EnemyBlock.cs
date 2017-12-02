using System;
using System.Linq;
using Data;
using UnityEngine;
using Random = System.Random;

namespace Components {
	public class EnemyBlock : MonoBehaviour {
		public FloatDataReference ScoreThreshold;
		private ScoreData _score;
		private int _childrenLeft;
		private BlocksData _blocksData;
		private int _counter;
		private readonly Random _rng = new Random();

		private void Awake() {
			_childrenLeft = GetComponentsInChildren<Enemy>().Length;
			_score = Resources.Load<ScoreData>("Global/Data/ScoreData");
			_blocksData = Resources.Load<BlocksData>("EnemyBlocks/AllBlocks");
			_blocksData.Blocks[GetNearest()] = _blocksData.Blocks[GetNearest()].OrderBy(x => _rng.Next()).ToList();
		}

		public void CheckBlockIsDone() {
			_childrenLeft--;
			if (_childrenLeft != 0)
				return;
			SpawnBlock();
			Destroy(gameObject);
		}

		private float GetNearest() {
			var thresholds = _blocksData.Blocks.Keys.ToList();
			var right = thresholds.Where(x => x >= _score.Score.Value).OrderBy(x => x - _score.Score.Value).First();
			var left = thresholds.Where(x => x < _score.Score.Value).OrderBy(x => Mathf.Abs(_score.Score.Value - x)).FirstOrDefault();
			return right.Equals(left) ? left : Mathf.Min(right, left);
		}

		public void SpawnBlock() {
			if (_counter >= _blocksData.Blocks[GetNearest()].Count) {
				_blocksData.Blocks[GetNearest()] = _blocksData.Blocks[GetNearest()].OrderBy(x => _rng.Next()).ToList();
				_counter = 0;
			}

			Instantiate(_blocksData.Blocks[GetNearest()][_counter]);
			_counter++;
		}
	}
}