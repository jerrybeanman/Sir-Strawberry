using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {
	public string itemName;
	public int itemID;
	public string itemDesc;
	public int itemPower;
	public int itemHp;
	public int itemMp;
	public Texture2D itemIcon;
	public ItemType itemType;

	//This bool shows wether if the current within the player's inventory 
	private bool exist = false;
	public bool Exist
	{
		get
		{
			return exist;
		}
	}
	//types of different items that we will have in our game
	public enum ItemType {
		Weapon,
		Potion,
		Armor
	}

	public Item(int id, string name, string desc, int power, int hp, int mp, ItemType type)
	{
		itemName = name;
		itemID = id;
		itemDesc = desc;
		itemPower = power;
		itemHp = hp;
		itemMp = mp;
		itemType = type;
		exist = true;
		itemIcon = Resources.Load<Texture2D>("Item Icons/" + name);
	}

	public Item(){	}
}
