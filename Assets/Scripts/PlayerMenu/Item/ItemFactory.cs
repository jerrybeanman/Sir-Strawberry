using UnityEngine;
using System.Collections;

public class ItemFactory : MonoBehaviour {

	public Item CreateItem(string type)
	{
		switch (type)
		{
		case "Item" :
			return new Item();
		case "Comsumable" :
			return new Comsumable();
		case "Weapon" :
			return new Weapon();
		case "Armor" :
			return new Armor();
		case "HeadPiece" :
			return new HeadPiece();
		case "ChestPiece" :
			return new ChestPiece();
		default :
			return null;
		}
	}

}
