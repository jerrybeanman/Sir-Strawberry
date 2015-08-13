using UnityEngine;
using System.Collections;
[System.Serializable]
public class Weapon : Item {

	public int weaponPower;
	public int weaponSpeed;

	public Weapon(int id, string name, string desc, int gold, int power, int speed) : base(id, name, desc, gold)
	{
		weaponPower = power;
		weaponSpeed = speed;
	}

	public override string ToString()
	{
		string s;
		s = base.ToString ();
		s += "<b><color=#4DA4BF>" +  "+ATK:" + "</color></b>" + weaponPower.ToString() + "\n";
		s += "<b><color=#4DA4BF>" +  "+SPD:" + "</color></b>" + weaponSpeed.ToString() + "\n";
		return s;
	}
}
