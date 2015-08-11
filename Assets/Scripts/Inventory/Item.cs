using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {
	public string itemName;
	public int itemID;
	public string itemDesc;
	public int itemPower;
	public int itemSpeed;
	public int itemHp;
	public Texture2D itemIcon;
	public ItemType itemType;
	[HideInInspector]public bool exist = false;
	
	public enum ItemType {
		Weapon,
		Consumable,
		Armor
	}

	public Item(string name, int id, string desc, int power, int speed, ItemType type)
	{
		itemName = name;
		itemID = id;
		itemDesc = desc;
		itemPower = power;
		itemSpeed = speed;
		itemType = type;
		exist = true;
		itemIcon = Resources.Load<Texture2D>("Item Icons/" + name);
	}

	public Item()
	{

	}
}
