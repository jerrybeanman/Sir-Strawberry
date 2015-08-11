using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	
	public void LoadLevel(int i)
	{
		Application.LoadLevel("inventoryTest" + i);
	}

	public void AddItem(int i)
	{
		Inventory.instance.AddItem(i);
	}
	
	public void ShowInventory()
	{
		Inventory.instance.setShowInventory();
	}
}