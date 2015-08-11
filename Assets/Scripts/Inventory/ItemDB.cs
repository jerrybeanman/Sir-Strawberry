using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDB : MonoBehaviour {
	public List<Item> items = new List<Item>();

	void Start()
	{
		items.Add (new Item("Blue Potion", 0, "gives mana", 0, 0, Item.ItemType.Consumable));
		items.Add (new Item("Silver Plate", 1, "plate made of silver", 20, 0, Item.ItemType.Weapon));
	}
}
