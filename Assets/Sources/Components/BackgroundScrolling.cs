using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Components {
	public class BackgroundScrolling : MonoBehaviour {
		[SerializeField]
		private float _speed;
		private Rigidbody2D _rigidbody2D;
		private List<SpriteRenderer> _backgroundParts;
		private Transform _transform;

		private void Awake() {
			_transform = GetComponent<Transform>();
			_backgroundParts = new List<SpriteRenderer>();
			for (var i = 0; i < _transform.childCount; i++) {
				var child = _transform.GetChild(i);
				var spriteRenderer = child.GetComponent<SpriteRenderer>();

				if (spriteRenderer != null) {
					_backgroundParts.Add(spriteRenderer);
				}
			}

			_backgroundParts = _backgroundParts.OrderBy(t => t.transform.position.x).ToList();
			foreach (var backgroundPart in _backgroundParts) {
				backgroundPart.GetComponent<Rigidbody2D>().velocity = new Vector2(_speed, 0f);
			}
		}

		private void Update() {
			var firstChild = _backgroundParts.FirstOrDefault();

			if (firstChild == null) {
				return;
			}

			if ((firstChild.transform.position.x >= Camera.main.transform.position.x) || firstChild.IsVisibleFrom(Camera.main) != false)
				return;
			var lastChild = _backgroundParts.LastOrDefault();
			var lastPosition = lastChild.transform.position;
			var lastSize = (lastChild.bounds.max - lastChild.bounds.min);

			firstChild.transform.position = new Vector3(lastPosition.x + lastSize.x,
				firstChild.transform.position.y,
				firstChild.transform.position.z);

			_backgroundParts.Remove(firstChild);
			_backgroundParts.Add(firstChild);
		}
	}

	public static class RendererExtensions {
		public static bool IsVisibleFrom(this Renderer renderer, Camera camera) {
			var planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
		}
	}
}