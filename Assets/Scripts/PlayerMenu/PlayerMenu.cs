using UnityEngine;
using System.Collections;

public abstract class PlayerMenu : MonoBehaviour {
	[System.Serializable]
	public class Position	{	public float xCor, yCor;	}
	[System.Serializable]
	public class Spacing	{	public float width, height;	}
	[System.Serializable]
	public class SlotSize	{	public float width, height;	}

	//position where the inventory is in the game
	public Position position;
	public SlotSize slotsize;

	//Some custom GUI styles for fun
	public GUISkin skin;
	private bool showPlayerMenu;
	
	//set to true when an item is pressed in the inventory menu
	protected bool showTooltip;
	//information to display in the tooltip box
	protected string tooltip;
	
	//set tot true when a item is currently being dragged. 
	protected bool draggingItem;
	//current item that is being dragged. 
	protected Item draggedItem;
	//The index where the item is being dragged off of so we can swap it with another item when dropped. *DOES NOT WORK*

	void OnGUI()
	{
		
		GUI.skin = skin;
		
		//pretty straightforward, not much to explain here
		if(showPlayerMenu)
		{
			Draw ();
			if(showTooltip)
				DrawTooltip();
		}
		
		if(draggingItem)
			DrawDraggingItem();
		
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

	public abstract void AddItem (int id);

	protected abstract void Draw();
	protected abstract void RemoveItem (int id);
	protected abstract void Drag(int i, Item item);
	protected abstract void DropSwap(int i);
	protected abstract void DropAssign(int i);
}
