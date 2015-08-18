using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {
	public int itemID;
	public string itemName;
	public string itemDesc;
	public int itemGold;

	//This bool shows wether if the current within the player's inventory 
	private bool exist = false;
	public bool Exist
	{
		get
		{
			return exist;
		}
	}
	
	public Item(int id, string name, string desc, int gold)
	{
		itemName = name;
		itemID = id;
		itemDesc = desc;
		exist = true;
	}

	virtual public string ToString()
	{
		string s;
		s = "<b><color=#4DA4BF>" + itemName + "</color></b>\n\n";
		s += "<b><color=#4DA4BF>" +  "Desc:" + "</color></b>" +  itemDesc + "\n";
		s += "<b><color=teal>" + itemGold.ToString () + "Gold" + "</color></b>\n\n";
		return s;
	}

	public Item(){	}
}
