using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager manager;
	public bool playersTurn = true;
	// wait ensures that the game waits before allowing the player to move again
	public bool wait = false;
	// enemiesMoving ensures that enemies wait enemyWaitTime time before moving again
	public bool enemiesMoving = false;
	// This is the time the enemies have to wait before executing another move
	public float enemyWaitTime = 1f;
	// This is the time the player has to wait before executing another move
	public float playerWaitTime = .2f;
	// List of enemy units. This is populated when an enemy is instantiated.
	public List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.

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

	// This creates test buttons to prove architecture.
	void OnGUI() {
		// Display health and experience on the screen.
		GUI.Label(new Rect(10,10,100,30), "health: " + Player.instance.playerStat.CurrentHp + "/" + Player.instance.playerStat.MaxHp);

		// Test save and load buttons.
		if (GUI.Button (new Rect (170, 10, 50, 25), "Save")) {
			Save();
		}
		if (GUI.Button (new Rect (170, 40, 50, 25), "Load")) {
			Load();
		}
	}

	private void Update() {
		// This logic is working but needs additional checks to prevent the player and enemy from ending up on the same tile and 'freezing'
		// If enemiesMoving is false, enter coroutine and start moving enemies
		if (!enemiesMoving) {
			StartCoroutine(WaitForEnemyMovement());
		}

		// If player is moving, or wait bool is toggled, return and do not allow player inputs
		if (playersTurn || wait) {
			return;
		}
		// If we are not waiting, and it is not players turn, wait (playerWaitTime) time
		StartCoroutine(WaitForPlayerMovement());
	}

	// Waits for the players movement to complete before accepting more input
	IEnumerator WaitForPlayerMovement() {
		wait = true;
		yield return new WaitForSeconds (playerWaitTime);
		playersTurn = true;
		wait = false;
	}

	// Allows each enemy to move once per time period
	IEnumerator WaitForEnemyMovement() {
		enemiesMoving = true;
		EnemyMovement ();
		yield return new WaitForSeconds (enemyWaitTime);
		enemiesMoving = false;
	}

	// Moves each enemy in the enemies list
	public void EnemyMovement() {
		foreach (Enemy en in enemies) {
			en.MoveEnemy();
		}
	}

	// Saves certain information to disk
	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		// This data path is for testing ONLY. Modify path if used on a different machine.
		FileStream file = File.Create(Application.persistentDataPath + "playerInfor.dat");

		// Creates our player data object
		PlayerData data = new PlayerData();
		// Transfers our current health and experience over to our new playerdata object.
		data.playerStat= Player.instance.playerStat;
		data.inventory = Inventory.instance.current;
		data.slots = Inventory.instance.slots;

		//  Serializes our data to our file.
		bf.Serialize(file, data);
		file.Close();
		print ("file saved");
	}

	// Loads certain information from disk
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
            Player.instance.playerStat = data.playerStat;

			Inventory.instance.current = data.inventory;
			Inventory.instance.slots = data.slots;
		} else {
			print("You do not have a saved file to load");
		}
	}
    // This class holds our persistent data. It it what is saved to local machine/server.
    // eg. items, health, level, experience, ...
    [Serializable]
    class PlayerData
    {
        public Player.PlayerStat playerStat;
        public List<Item> inventory;
        public List<Item> slots;
    }

}