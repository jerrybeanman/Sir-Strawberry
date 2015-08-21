using UnityEngine;
using System.Collections;
using System;

public class Inventory : PlayerMenu{
	//Used for singleton design pattern
	public static Inventory instance = null;

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
		isInventory = true;
		for(int i = 0; i < width * height; i++)
		{
			//initialize slots with empty items
			slots.Add (new Item());
			//initialize an empty inventory
			current.Add (new Item());
		}
	}

    protected override Rect DrawSlots(int x, int y)
    {
        //position to draw Empty slots and items. This is scaled so to the size of the screen so it is platform independent
        Rect slotRect = new Rect(Screen.width / position.xCor + x * Screen.width / spacing.width,
                                 Screen.height / position.yCor + y * Screen.width / spacing.height,
                                 Screen.width / slotsize.width,
                                 Screen.height / slotsize.height);
            GUI.Box(slotRect, "", skin.GetStyle("Slot"));
        return slotRect;
    }

    protected override void DrawStat() { }
}



