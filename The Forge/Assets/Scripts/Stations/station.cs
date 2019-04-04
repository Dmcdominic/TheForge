using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic station, used to craft certain items into other items
/// </summary>
[CreateAssetMenu(menuName = "station")]
public class station : ScriptableObject {
	
	public Sprite icon;
	public bool crate;

	private List<item> _products;
	public List<item> products {
		get {
			if (_products == null) {
				_products = items_oracle.all.FindAll(x => x.station == this);
			}
			return _products;
		}
	}

	// Reset
	private void OnEnable() {
		_products = null;
	}
}
