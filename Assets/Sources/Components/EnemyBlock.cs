﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using Random = System.Random;

namespace Components {
	public class EnemyBlock : MonoBehaviour {
		public EnemyBlock[] TutorialBlocks;
		public FloatDataReference ScoreThreshold;
		[SerializeField]
		private float _delay;
		private ScoreData _score;
		private int _childrenLeft;
		private BlocksData _blocksData;
		private int _counter;
		private readonly Random _rng = new Random();
		private GameEvent _spawnBlockEvent;
		private int _tutorial;

		private void Awake() {
			_childrenLeft = GetComponentsInChildren<Enemy>().Length;
			_score = Resources.Load<ScoreData>("Global/Data/ScoreData");
			_blocksData = Resources.Load<BlocksData>("EnemyBlocks/AllBlocks");
			_blocksData.Blocks[GetNearest()] = _blocksData.Blocks[GetNearest()].OrderBy(x => _rng.Next()).ToList();
			_spawnBlockEvent = Resources.Load<GameEvent>("EnemyBlocks/Events/SpawnBlock");
		}

		public void CheckBlockIsDone() {
			_childrenLeft--;
			if (_childrenLeft != 0)
				return;

			SpawnBlock();
			_spawnBlockEvent.Raise();
		}

		private float GetNearest() {
			var thresholds = _blocksData.Blocks.Keys.ToList();
			var right = thresholds.Where(x => x >= _score.Score.Value).OrderBy(x => x - _score.Score.Value).First();
			var left = thresholds.Where(x => x < _score.Score.Value).OrderBy(x => Mathf.Abs(_score.Score.Value - x)).FirstOrDefault();
			return right.Equals(left) ? left : Mathf.Min(right, left);
		}

		public void SpawnBlock() {
			StartCoroutine(_tutorial < TutorialBlocks.Length ? SpawnTutorial() : SpawnBlockAfterDelay());
		}

		public IEnumerator SpawnTutorial() {
			Instantiate(TutorialBlocks[_tutorial]);
			_tutorial++;
			yield return null;
		}

		public IEnumerator SpawnBlockAfterDelay() {
			yield return new WaitForSeconds(_delay);
			if (_counter >= _blocksData.Blocks[GetNearest()].Count) {
				_blocksData.Blocks[GetNearest()] = _blocksData.Blocks[GetNearest()].OrderBy(x => _rng.Next()).ToList();
				_counter = 0;
			}

			Instantiate(_blocksData.Blocks[GetNearest()][_counter]);
			_counter++;
			Destroy(gameObject);
		}
	}
}