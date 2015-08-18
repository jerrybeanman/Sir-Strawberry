using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class PlayerMenu : MonoBehaviour {
	[System.Serializable]
	public class Position	{	public float xCor, yCor;	}
	[System.Serializable]
	public class Spacing	{	public float width, height;	}
	[System.Serializable]
	public class SlotSize	{	public float width, height;	}

	public float BackGroundYCor;
	public float BackGroundHeight;
	//position where the inventory is in the game
	public Position position;
	//spacing between each slot
	public Spacing spacing;
	public SlotSize slotsize;

	//contains valid/existing items that the player already have
	[HideInInspector]public List<Item> current = new List<Item>();
	//the total number of slots in the inventory, each slot will have a item (if not, Item.Exist is default to false) so we can manipulate with the inventory
	[HideInInspector]public List<Item> slots = new List<Item>();

	//Some custom GUI styles for fun
	public GUISkin skin;
	private bool showPlayerMenu;
	
	//set to true when an item is pressed in the inventory menu
	protected bool showTooltip;
	//information to display in the tooltip box
	protected string tooltip;
	
	//set tot true when a item is currently being dragged. 
	[HideInInspector]public static bool draggingItem;
	//current item that is being dragged. 
	[HideInInspector]public static Item draggedItem;
	//The index where the item is being dragged off of so we can swap it with another item when dropped. *DOES NOT WORK*
	protected int prevIndex;

	public int width, height;

	protected bool isInventory;
	void OnGUI()
	{
		
		GUI.skin = skin;
		
		//pretty straightforward, not much to explain here
		if(showPlayerMenu)
		{
			GUI.Box(new Rect(1, Screen.height / BackGroundYCor, Screen.width, Screen.height / BackGroundHeight), 
			        "", skin.GetStyle("Background"));
			Draw ();
			if(showTooltip)
				DrawTooltip();
		}
		
		if(draggingItem)
			DrawDraggingItem();
		
	}

	protected void Draw()
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
				Rect slotRect = new Rect(Screen.width / position.xCor + x * Screen.width / spacing.width, 
				                         Screen.height / position.yCor + y * Screen.width / spacing.height, 
				                         Screen.width / slotsize.width, 
				                         Screen.height / slotsize.height);
				if(!isInventory)
				{
					if((x != 0 || y != 0) && (x != width -1 || y != 0) && (x != width -2 || y != height -1))
						GUI.Box (slotRect, "", skin.GetStyle ("Slot"));
				}else
					GUI.Box (slotRect, "", skin.GetStyle ("Slot"));
				
				//assign all item in the inventory to slots so we can keep track what each slot contains
				slots[i] = current[i];
				//Current item
				Item item = slots[i];
				//check if an existing item is assigned to slot
				if(item.Exist)
				{
					//draw the item
					GUI.DrawTexture (slotRect, Resources.Load<Texture2D>("Item Icons/" + item.itemName));
					
					//Check if mouse is hovering over an existing item
					if(slotRect.Contains(e.mousePosition))
					{
						tooltip = item.ToString();
						showTooltip = true;
						
						//if mouse is right clicked on the item
						if(e.isMouse && e.type == EventType.mouseDown && e.button == 1 && isInventory)
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
				//if dragged item is hovering over an empty slot and being released
				if(slotRect.Contains(e.mousePosition) && e.type == EventType.mouseUp && draggingItem){
					DropAssign (i);
				}
				i++;
			}
		}
	}

	public void setShowPlayerMenu()
	{
		showPlayerMenu = !showPlayerMenu;
	}

	protected void DrawDraggingItem()
	{
		//draw the item texture while dragging at a position 15x, 15y pixels relative to the mouse position
		GUI.DrawTexture(new Rect(Event.current.mousePosition.x + 15, Event.current.mousePosition.y, 50, 50), Resources.Load<Texture2D>("Item Icons/" + draggedItem.itemName));
	}
	
	protected void DrawTooltip()
	{
		GUI.Box (new Rect(Screen.width / 2.5f, Screen.height / 1.65f, Screen.width / 1.75f, Screen.height / 3.75f), tooltip, skin.GetStyle("Tooltip"));
	}

	public virtual void Drag(int i, Item item)
	{
		draggingItem = true;
		prevIndex = i;
		draggedItem = item;
		
		//delete the item by making it an empty item
		current[i] = new Item();
	}
	
	public virtual void DropSwap(int i)
	{
		current[prevIndex] = current[i];
		current[i] = draggedItem;
		draggingItem = false;
		draggedItem = null;
	}
	
	public virtual void DropAssign(int i)
	{
			current [i] = draggedItem;
			draggingItem = false;
			draggedItem = null;
	}
	
	
	public void AddItem(int id)
	{
		for(int i = 0; i < current.Count; i++)
		{
			if(!current[i].Exist)
			{
				for(int j = 0; j < ItemDB.instance.Items.Count; j++)
				{
					if(ItemDB.instance.Items[j].itemID == id)
						current[i] = ItemDB.instance.Items[j];
				}
				break;
			}
		}
	}
	
	public void RemoveItem(int id)
	{
		for(int i = 0; i < current.Count; i++)
		{
			if(current[i].itemID == id)
			{
				current[i] = new Item();
				break;
			}
		}
	}
	
	
	
	
	//Check if the inventory contains an item with the corresponding id
	// *NOT USED YET*
	private bool Contain(int id)
	{
		for(int i = 0; i < current.Count; i++)
			if(current[i].itemID == id)
				return true;
		return false;
	}
	
	private void EquipWeapon(Item item, int i)
	{
		print ("Weapon Equipped");
		current[i] = new Item();
	}
	private void EquipArmor(Item item, int i){
		print ("Armor Euipped");
		current[i] = new Item();
	}
	private void EquipPotion(Item item, int i){
		print ("Potion Euipped");
		current[i] = new Item();
	}

}
