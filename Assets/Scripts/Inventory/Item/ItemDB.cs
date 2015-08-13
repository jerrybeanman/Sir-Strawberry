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
		items.Add (new Comsumable(0, "Blue Potion", "gives mana banana", 15, 0, 20));
		items.Add (new Armor(1, "Silver Plate", "plate madse of silver", 200, 20));
		items.Add (new Weapon(2, "Dagger", "Daggur muh niggur", 100, 20, 5));
	}

}
