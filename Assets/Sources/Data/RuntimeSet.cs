using System.Collections.Generic;
using UnityEngine;

namespace Data {
	public abstract class RuntimeSet<T> : ScriptableObject {
		public List<T> Items = new List<T>();

		public void Add(T obj) {
			if (!Items.Contains(obj)) {
				Items.Add(obj);
			}
		}

		public void Remove(T obj) {
			if (Items.Contains(obj)) {
				Items.Remove(obj);
			}
		}
	}
}