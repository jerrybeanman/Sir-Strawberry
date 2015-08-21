using UnityEngine;
using System.Collections;

public class Armory : PlayerMenu {
	public static Armory instance = null;

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
		for (int i = 0; i < width * height; i++)
			slots.Add (new Item ());
		current.Add (new Item ());
		current.Add (new HeadPiece ());
		current.Add (new Item ());
		current.Add (new Weapon ());
		current.Add (new ChestPiece ());
		current.Add (new Weapon ());
		current.Add (new Comsumable ());
		current.Add (new Item ());
		current.Add (new Comsumable ());
        
	}
	
	protected override void Drag(int i, Item item)
	{
        current[i].RemoveStat();
		base.Drag (i, item);
	}
	
	protected override void DropSwap(int i)
	{
		base.DropSwap (i);
	}
	
	protected override void DropAssign(int i)
	{
        print(current[i].GetType() + " " + draggedItem.GetType());
        if (current[i].GetType() == draggedItem.GetType())
        {
            base.DropAssign(i);
            current[i].AddStat();
        }
    }

    protected override Rect DrawSlots(int x, int y)
    {
        //position to draw Empty slots and items. This is scaled so to the size of the screen so it is platform independent
        Rect slotRect = new Rect(Screen.width / position.xCor + x * Screen.width / spacing.width,
                                 Screen.height / position.yCor + y * Screen.width / spacing.height,
                                 Screen.width / slotsize.width,
                                 Screen.height / slotsize.height);
        if ((x != 0 || y != 0) && (x != width - 1 || y != 0) && (x != width - 2 || y != height - 1))
            GUI.Box(slotRect, "", skin.GetStyle("Slot"));
        return slotRect;
    }

    protected override void DrawStat()
    {
        GUI.Box(new Rect(Screen.width / PlayerStatValues.position.xCor,
                          Screen.height / PlayerStatValues.position.yCor,
                          Screen.width / PlayerStatValues.slotsize.width,
                          Screen.height / PlayerStatValues.slotsize.height), Player.instance.StatToString(), 
                          skin.GetStyle("Tooltip"));
    }

}
