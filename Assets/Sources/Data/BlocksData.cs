﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Components;
using UnityEngine;

namespace Data {
	[CreateAssetMenu(fileName = "BlocksData", menuName = "Data/BlocksData", order = 0)]
	public class BlocksData : ScriptableObject {
		public Dictionary<float, List<EnemyBlock>> Blocks;
		public ScoreData ScoreData;

		private void OnEnable() {
			PopulateBlocksData();
		}

		public void PopulateBlocksData() {
			var blocks = Resources.LoadAll<EnemyBlock>("EnemyBlocks/Blocks");
			Blocks = new Dictionary<float, List<EnemyBlock>>();
			foreach (var block in blocks) {
				if (Blocks.ContainsKey(block.ScoreThreshold.Value)) {
					Blocks[block.ScoreThreshold.Value].Add(block);
				} else {
					Blocks[block.ScoreThreshold.Value] = new List<EnemyBlock> { block };
				}
			}
		}
	}
}