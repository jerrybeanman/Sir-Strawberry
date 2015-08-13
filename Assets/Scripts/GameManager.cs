using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager manager;
	public float health;
	public float experience;

	// Use this for initialization
	void Awake () {
		// Singleton design pattern. If gameManager doesnt exist, this becomes our manager.
		// If it does exist, do not overwrite it.
		if (manager == null) {
			DontDestroyOnLoad (gameObject);
			manager = this;
		} else if (manager != this) {
			Destroy(gameObject);
		}

	}
	
	void OnGUI() {
		// Display health and experience on the screen.
		GUI.Label(new Rect(10,10,100,30), "health: " + health);
		GUI.Label(new Rect(10,40,100,30), "experience: " + experience);

		// Test save and load buttons.
		if (GUI.Button (new Rect (170, 10, 50, 25), "Save")) {
			Save();
		}
		if (GUI.Button (new Rect (170, 40, 50, 25), "Load")) {
			Load();
		}
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		// This data path is for testing ONLY. Modify path if used on a different machine.
		FileStream file = File.Create(Application.persistentDataPath + "playerInfor.dat");

		// Creates our player data object
		PlayerData data = new PlayerData();
		// Transfers our current health and experience over to our new playerdata object.
		data.health = health;
		data.experience = experience;
		data.inventory = Inventory.instance.inventory;
		data.slots = Inventory.instance.slots;

		//  Serializes our data to our file.
		bf.Serialize(file, data);
		file.Close();
		print ("file saved");
	}

	public void Load() {
		string filePath = Application.persistentDataPath + "playerInfor.dat";
		// Again, path needs to be changed if used on a different machine.
		if (File.Exists (filePath)) {
			print ("loading");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (filePath, FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);
			file.Close ();
			// Transfers our saved data back to our gameManager object.
			health = data.health;
			experience = data.experience;

			Inventory.instance.inventory = data.inventory;
			Inventory.instance.inventory = data.slots;
		} else {
			print("You do not have a saved file to load");
		}
	}


}

// This class holds our persistent data. It it what is saved to local machine/server.
// eg. items, health, level, experience, ...
[Serializable]
class PlayerData {
	public float health;
	public float experience;
	public List<Item> inventory;
	public List<Item> slots;

}