using UnityEngine;
using System.Collections;

public class HavingFun : MonoBehaviour {

	public void LoadLevel(int i)
	{
		Application.LoadLevel("inventoryTest" + i);
	}

	public void AddPotion()
	{
		Inventory.instance.AddItem(0);
	}

	public void AddArmor()
	{
		Inventory.instance.AddItem(1);
	}

	public void AddDagger()
	{
		Inventory.instance.AddItem(2);
	}

	public void ShowInventory()
	{
		Inventory.instance.setShowInventory();
	}
}