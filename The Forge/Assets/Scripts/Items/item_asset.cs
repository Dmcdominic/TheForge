using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ScriptableObject used to generate and store the properties of a particular item
/// </summary>
[CreateAssetMenu(menuName="item")]
public class item_asset : ScriptableObject {
	public item Item;
}
