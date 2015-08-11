using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	public static Inventory instance = null;
	public int width, height;
	public GUISkin skin;

	//contains valid/existing items that the player already have
	private List<Item> inventory = new List<Item>();
	//the total number of slots in the inventory, each slot will have a item (if not, Item.Exist is default to false) so we can manipulate with the inventory
	private List<Item> slots = new List<Item>();
	private bool showInventory;
	private bool showTooltip;
	private string tooltip;

	private bool draggingItem;
	private Item draggedItem;
	private int prevIndex;

	void Awake()
	{
		if(instance == null)
			instance = this;
		else if(instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start(){
		for(int i = 0; i < width * height; i++)
		{
			slots.Add (new Item());
			inventory.Add (new Item());
		}

	}

	public void setShowInventory()
	{
		showInventory = !showInventory;
	}

	
	void OnGUI()
	{
		
		GUI.skin = skin;

		if(showInventory)
		{
			DrawInventory ();
			if(showTooltip)
				DrawTooltip();
		}

		/*
		 * not working
		if(draggingItem)
			DrawDraggingItem();
		*/

		//displaying current inventory items as text whether if the inventory is open or not.
		// *for testing purposes*
		for (int i = 0; i < inventory.Count; i++) 
			GUI.Label (new Rect(10,i * 20,200,50), inventory[i].itemName);
	}

	void DrawInventory()
	{
		//stores information of current events, so it allows us to capture mouse position and enable drag & drop functionality
		Event e = Event.current;

		//keeps track of total slots
		int i = 0;

		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				Rect slotRect = new Rect(Screen.width / 3 + x * Screen.width / 7, Screen.height / 9.6f + y * Screen.height / 10, Screen.width / 8, Screen.height / 12);
				//draw inventory
				GUI.Box (slotRect, "", skin.GetStyle ("Slot"));

				//assign all item in the inventory to slots so we can keep track what each slot contains
				slots[i] = inventory[i];
				//check if an existing item is assigned to slot
				if(slots[i].Exist)
				{
					Item item = slots[i];
					//draw the item
					GUI.DrawTexture (slotRect, item.itemIcon);

					//Check if mouse is hovering over an existing item
					if(slotRect.Contains(e.mousePosition))
					{
						//if mouse is left clicked on the item
						if(Input.GetMouseButtonDown(0))
						{
							GenerateTooltip(item);
							showTooltip = true;
						}

						//if mouse is right clicked on the item
						if(e.isMouse && e.type == EventType.mouseDown && e.button == 1)
						{
							if(item.itemType == Item.ItemType.Weapon)
							{
								EquipWeapon(i);
							}

							if(item.itemType == Item.ItemType.Armor)
							{
								EquipArmor(i);
							}
						}

						/**
						 * drag & drop feature, currently not working. 
						//Check if mouse is clicked on an existing item and it is "dragged"
						if(e.button ==  0 && e.type == EventType.mouseDrag && !draggingItem)
						{
							draggingItem = true;
							prevIndex = i;
							draggedItem = slots[i];

							//delete the item by making it an empty item
							inventory[i] = new Item();
						}

						//Check if mouse is release while draggging an item
						if(e.type == EventType.mouseUp && draggingItem)
						{
							inventory[prevIndex] = inventory[i];
							inventory[i] = draggedItem;
							draggingItem = false;
							draggedItem = null;
						}*/
					} 
				}
				i++;
			}
		}
	}

	//Populate the tool tip string with the corresponding item information
	void GenerateTooltip(Item item)
	{
		tooltip = "<b><color=#4DA4BF>" + item.itemName + "</color></b>\n\n";
		tooltip += "<b><color=#4DA4BF>" +  "Desc:" + "</color></b>" +  item.itemDesc + "\n\n";
		tooltip += "<b><color=#4DA4BF>" +  "ATK:" + "</color></b>" + item.itemPower.ToString() + "\n";
		tooltip += "<b><color=#4DA4BF>" +  "HP:" + "</color></b>" + item.itemHp.ToString() + "\n";
		tooltip += "<b><color=#4DA4BF>" +  "MANA:" + "</color></b>" + item.itemMp.ToString() + "\n";
	}

	void DrawDraggingItem()
	{
		//draw the item texture while dragging at a position 15x, 15y pixels relative to the mouse position
		GUI.DrawTexture(new Rect(Event.current.mousePosition.x + 15, Event.current.mousePosition.y, 50, 50), draggedItem.itemIcon);
	}

	void DrawTooltip()
	{
		GUI.Box (new Rect(110, 300, 200, 125), tooltip, skin.GetStyle("Tooltip"));
	}

	public void AddItem(int id)
	{
		print ("1");
		for(int i = 0; i < inventory.Count; i++)
		{
			if(!inventory[i].Exist)
			{
				for(int j = 0; j < ItemDB.instance.Items.Count; j++)
				{
					if(ItemDB.instance.Items[j].itemID == id)
						inventory[i] = ItemDB.instance.Items[j];
				}
				break;
			}
		}
	}

	void RemoveItem(int id)
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			if(inventory[i].itemID == id)
			{
				inventory[i] = new Item();
				break;
			}
		}
	}




	//Check if the inventory contains an item with the corresponding id
	// *not used yet*
	bool Contain(int id)
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			if(inventory[i].itemID == id)
				return true;
		}
		return false;
	}

	private void EquipWeapon(int i)
	{
		print ("Weapon Equipped");
		inventory[i] = new Item();
	}
	private void EquipArmor(int i){
		print ("Armor Euipped");
		inventory[i] = new Item();
	}
}



