using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ItemListStorage : SerializableDictionary.Storage<List<item_id>> { }

[Serializable]
public class ItemID_ItemIDList_Dictionary : SerializableDictionary<item_id, List<item_id>, ItemListStorage> { }
