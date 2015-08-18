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
	
	public override void Drag(int i, Item item)
	{
		base.Drag (i, item);
	}
	
	public override void DropSwap(int i)
	{
		base.DropSwap (i);
	}
	
	public override void DropAssign(int i)
	{
		if(current[i].GetType() == draggedItem.GetType())
			base.DropAssign (i);
	}


}
