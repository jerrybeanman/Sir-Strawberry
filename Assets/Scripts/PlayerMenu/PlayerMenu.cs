using UnityEngine;
using System.Collections;

public class PlayerMenu : MonoBehaviour {
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
	[System.Serializable]
	public class SlotSize
	{
		public float width;
		public float height;
	}

	//position where the inventory is in the game
	public Position position;
	public SlotSize slotsize;

	//Some custom GUI styles for fun
	public GUISkin skin;

	//set to true when an item is pressed in the inventory menu
	protected bool showTooltip;
	//information to display in the tooltip box
	protected string tooltip;

	
	//set tot true when a item is currently being dragged. 
	protected bool draggingItem;
	//current item that is being dragged. 
	protected Item draggedItem;
	//The index where the item is being dragged off of so we can swap it with another item when dropped. *DOES NOT WORK*
}
