using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	public int width, height;
	public GUISkin skin;
	public List<Item> inventory = new List<Item>();
	public List<Item> slots = new List<Item>();
	private ItemDB db;
	private bool showInventory = false;

	// Use this for initialization
	void Start(){
		for(int i = 0; i < width * height; i++)
		{
			slots.Add (new Item());
			inventory.Add (new Item());
		}
		db = GameObject.FindGameObjectWithTag ("itemDB").GetComponent<ItemDB>();
		inventory[0] = db.items[0];
		inventory[1] = db.items[1];
	}

	public void setShowInventory()
	{
		showInventory = !showInventory;
	}

	void OnGUI()
	{
		GUI.skin = skin;
		if(showInventory)
			DrawInventory ();
		for (int i = 0; i < inventory.Count; i++) 
		{
			GUI.Label (new Rect(10,i * 20,200,50), inventory[i].itemName);
		}
	}

	void DrawInventory()
	{
		int i = 0;
		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				Rect slotRect = new Rect(50 + x * 60,50 + y * 60, 50, 50);
				GUI.Box (slotRect, "", skin.GetStyle ("Slot"));

				slots[i] = inventory[i];
				if(slots[i].exist)
				{
					GUI.DrawTexture (new Rect(50 + x * 60, 20 + y * 60, 80, 80), slots[i].itemIcon);
				}
				i++;
			}
		}
	}
}
