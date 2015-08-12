using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDB : MonoBehaviour {
	public static ItemDB instance = null;
	private List<Item> items = new List<Item>();

	void Awake()
	{
		if(instance == null)
			instance = this;
		else if(instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad(gameObject);
	}

	//items should be encapsulated so its READ only
	public List<Item> Items
	{
		get
		{
			return items;
		}
	}

	//Generate some data for testing purposes.
	//Can also try reading item informations through a xml file
	void Start()
	{
		items.Add (new Item(0, "Blue Potion", "gives mana banana", 0, 0, 20, Item.ItemType.Potion));
		items.Add (new Item(1, "Silver Plate", "plate madse of silver", 0, 20, 0, Item.ItemType.Weapon));
		items.Add (new Item(2, "Dagger", "Daggur muh niggur", 20, 0, 0, Item.ItemType.Weapon));
	}
}
