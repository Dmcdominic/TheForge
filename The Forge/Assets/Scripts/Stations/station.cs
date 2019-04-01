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
	public List<item> products;

}
