using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ScriptableObject used to generate and store the properties of a particular station
/// </summary>
[CreateAssetMenu(menuName = "station")]
public class station_asset : ScriptableObject {
	public station Station;
}
