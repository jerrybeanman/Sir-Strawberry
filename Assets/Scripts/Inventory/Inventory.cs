using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	[System.Serializable]
	public class Position
	{
		public float xCor;
		public float yCor;
	}
	[System.Serializable]
	public class Spacing
	{
		public float width;
		public float height;
	}
	//position where the inventory is in the game
	public Position position;
	//spacing between each slot
	public Spacing spacing;
	//Used for singleton design pattern
	public static Inventory instance = null;
	//the grid size of the inventory
	public int width, height;
	//Some custom GUI styles for fun
	public GUISkin skin;

	//contains valid/existing items that the player already have
	private List<Item> inventory = new List<Item>();
	//the total number of slots in the inventory, each slot will have a item (if not, Item.Exist is default to false) so we can manipulate with the inventory
	private List<Item> slots = new List<Item>();
	//set to true when inventory button is pressed
	private bool showInventory;
	//set to true when an item is pressed in the inventory menu
	private bool showTooltip;
	//information to display in the tooltip box
	private string tooltip;

	//set tot true when a item is currently being dragged. *DOES NOT WORK*
	private bool draggingItem;
	//current item that is being dragged. *DOES NOT WORK*
	private Item draggedItem;
	//The index where the item is being dragged off of so we can swap it with another item when dropped. *DOES NOT WORK*
	private int prevIndex;


	void Awake()
	{
		//singleton design pattern
		if(instance == null)
			instance = this;
		else if(instance != this)
			Destroy (gameObject);

		//since player inventory should persist over scenes, this object should not be destroyed on load
		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start(){
		for(int i = 0; i < width * height; i++)
		{
			//initialize slots with empty items
			slots.Add (new Item());
			//initialize an empty inventory
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

		//pretty straightforward, not much to explain here
		if(showInventory)
		{
			DrawInventory ();
			if(showTooltip)
				DrawTooltip();
		}

		if(draggingItem)
			DrawDraggingItem();
	}

	void DrawInventory()
	{
		//stores information of current events, so it allows us to capture mouse position and enable drag & drop functionality
		Event e = Event.current;

		//Since x and y cannot specify where the current slot is at, we need a variable to keep track when iterating through
		int i = 0;

		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				//position to draw Empty slots and items. This is scaled so to the size of the screen so it is platform independent
				Rect slotRect = new Rect(Screen.width / position.xCor + x * Screen.width / spacing.width, Screen.height / position.yCor + y * Screen.height / spacing.height, Screen.width / 8, Screen.height / 12);
				//draw inventory
				GUI.Box (slotRect, "", skin.GetStyle ("Slot"));

				//assign all item in the inventory to slots so we can keep track what each slot contains
				slots[i] = inventory[i];
				//Current item
				Item item = slots[i];

				//check if an existing item is assigned to slot
				if(item.Exist)
				{
					//draw the item
					GUI.DrawTexture (slotRect, Resources.Load<Texture2D>("Item Icons/" + item.itemName));

					//Check if mouse is hovering over an existing item
					if(slotRect.Contains(e.mousePosition) )//&& Input.GetMouseButtonDown(0))
					{
						GenerateTooltip(item);
						showTooltip = true;

						//if mouse is right clicked on the item
						if(e.isMouse && e.type == EventType.mouseDown && e.button == 1)
						{
							if(item.itemType == Item.ItemType.Weapon)
								EquipWeapon(item, i);

							if(item.itemType == Item.ItemType.Armor)
								EquipArmor(item, i);

							if(item.itemType == Item.ItemType.Potion)
								EquipPotion(item, i);
						}

			
						// drag & drop feature *CURRENTLY NOT WORKING* 
						//Check if mouse is clicked on an existing item and it is "dragged"
						if(e.button == 0 && e.type == EventType.mouseDrag && !draggingItem)
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
						}
					} 
				}else 
				//if dragged item is hovering over an empty slot
				if(slotRect.Contains(e.mousePosition) && e.type == EventType.mouseUp && draggingItem)
				{
					inventory[i] = draggedItem;
					draggingItem = false;
					draggedItem = null;
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
		GUI.Box (new Rect(Screen.width / 2.5f, Screen.height / 1.65f, Screen.width / 1.75f, Screen.height / 3.75f), tooltip, skin.GetStyle("Tooltip"));
	}

	public void AddItem(int id)
	{
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
	// *NOT USED YET*
	bool Contain(int id)
	{
		for(int i = 0; i < inventory.Count; i++)
		{
			if(inventory[i].itemID == id)
				return true;
		}
		return false;
	}

	private void EquipWeapon(Item item, int i)
	{
		print ("Weapon Equipped");
		inventory[i] = new Item();
	}
	private void EquipArmor(Item item, int i){
		print ("Armor Euipped");
		inventory[i] = new Item();
	}
	private void EquipPotion(Item item, int i){
		print ("Potion Euipped");
		inventory[i] = new Item();
	}
}



