using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : PlayerMenu{
	//spacing between each slot
	public Spacing spacing;
	//Used for singleton design pattern
	public static Inventory instance = null;
	//the grid size of the inventory
	public int width, height;

	//contains valid/existing items that the player already have
	public List<Item> inventory = new List<Item>();
	//the total number of slots in the inventory, each slot will have a item (if not, Item.Exist is default to false) so we can manipulate with the inventory
	public List<Item> slots = new List<Item>();
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
	



	protected override void Draw()
	{
		//stores information of current events, so it allows us to capture mouse position and enable drag & drop functionality
		Event e = Event.current;

		//Since x and y cannot specify where the current slot is at, we need a variable to keep track when iterating through
		int i = 0;

	Rect backGround = new Rect (Screen.width / position.xCor - Screen.width / spacing.width + Screen.width / slotsize.width, 
		                        Screen.height / position.yCor - Screen.height / spacing.height + Screen.height / slotsize.height, 
		                        Screen.width / slotsize.width * width + Screen.width / slotsize.width, 
		                        Screen.height / slotsize.height * height + Screen.height / slotsize.width);
		GUI.Box (backGround, "", skin.GetStyle ("Inventory"));
		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				//position to draw Empty slots and items. This is scaled so to the size of the screen so it is platform independent
				Rect slotRect = new Rect(Screen.width / position.xCor + x * Screen.width / spacing.width, 
				                         Screen.height / position.yCor + y * Screen.width / spacing.height, 
				                         Screen.width / slotsize.width, 
				                         Screen.height / slotsize.height);
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
						tooltip = item.ToString();
						showTooltip = true;

						//if mouse is right clicked on the item
						if(e.isMouse && e.type == EventType.mouseDown && e.button == 1)
						{
							if(item is Weapon)
								EquipWeapon(item, i);

							if(item is Armor)
								EquipArmor(item, i);

							if(item is Comsumable)
								EquipPotion(item, i);
						}
						 
						//Check if mouse is clicked on an existing item and it is "dragged"
						if(e.button == 0 && e.type == EventType.mouseDrag && !draggingItem)
							Drag (i, item);

						//Check if mouse is release while draggging an item on an existing item
						if(e.type == EventType.mouseUp && draggingItem)
							DropSwap(i);
						
					} 
				}else 
				//if dragged item is hovering over an empty slot
				if(slotRect.Contains(e.mousePosition) && e.type == EventType.mouseUp && draggingItem)
						DropAssign (i);
				i++;
			}
		}
	}

	protected override void Drag(int i, Item item)
	{
		draggingItem = true;
		prevIndex = i;
		draggedItem = item;
		
		//delete the item by making it an empty item
		inventory[i] = new Item();
	}

	protected override void DropSwap(int i)
	{
		inventory[prevIndex] = inventory[i];
		inventory[i] = draggedItem;
		draggingItem = false;
		draggedItem = null;
	}

	protected override void DropAssign(int i)
	{
		inventory[i] = draggedItem;
		draggingItem = false;
		draggedItem = null;
	}
	

	public override void AddItem(int id)
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

	protected override void RemoveItem(int id)
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



