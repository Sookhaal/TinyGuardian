﻿using System.Collections.Generic;
using System.Text;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Components {
	public class TextReplacer : MonoBehaviour {
		public FloatData FloatValue;
		[SerializeField]
		private Color _scoreColor;
		[SerializeField]
		private Color _zeroColor;
		[SerializeField]
		private int _padding = 8;
		private Text _text;
		private string _scoreString;
		private StringBuilder _builder;
		[SerializeField]
		private string _prefix;

		private void Awake() {
			_text = GetComponent<Text>();
			_builder = new StringBuilder();
		}

		private void Update() {
			_scoreString = _prefix;
			_builder.Clear();
			_builder.Append(_scoreString);
			for (var i = 0; i < _padding - FloatValue.Value.ToString("0").Length; i++) {
				_builder.Append("0");
			}
			_scoreString = _builder.ToString();
			_text.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_zeroColor)}>{_scoreString}</color><color=#{ColorUtility.ToHtmlStringRGBA(_scoreColor)}>{FloatValue.Value:0}</color>";
		}
	}
}