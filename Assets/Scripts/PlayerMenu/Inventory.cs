using UnityEngine;
using System.Collections;

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


}



