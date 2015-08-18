using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Armory : Inventory {
	public static Armory instance;
	public int HeadPiecePostion;
	
	[HideInInspector]public List<Item> armory = new List<Item>();

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

	void Start()
	{
		armory.Add (new HeadPiece ());
		armory.Add (new ChestPiece ());
		armory.Add (new Weapon ());
		armory.Add (new Weapon ());
		armory.Add (new Comsumable ());
		armory.Add (new Comsumable ());
	}

	/*protected override void Draw()
	{
		//stores information of current events, so it allows us to capture mouse position and enable drag & drop functionality
		Event e = Event.current;
		
		//Since x and y cannot specify where the current slot is at, we need a variable to keep track when iterating through
		int i = 0;
		
		
		for (int y = 0; y < height; y++) 
		{
			for (int x = 0; x < width; x++) 
			{
				//position to draw Empty slots and items. This is scaled so to the size of the screen so it is platform independent
				Rect slotRect = new Rect(Screen.width / position.xCor + x * Screen.width / spacing.width, 
				                         Screen.height / position.yCor + y * Screen.width / spacing.height, 
				                         Screen.width / slotsize.width, 
				                         Screen.height / slotsize.height);
				//draw inventory
				GUI.Box (slotRect, "", skin.GetStyle ("Slot"));
			}
		}
	}*/

	
	public class Pair<T, U> {
		public Pair() {}
	
		public Pair(T first, U second) {
			this.First = first;
			this.Second = second;
		}
	
		public T First { get; set; }
		public U Second { get; set; }
	};
}
