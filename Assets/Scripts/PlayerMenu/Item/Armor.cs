using UnityEngine;
using System.Collections;
[System.Serializable]
public class Armor : Item {

	public int armorHp;

	public override string Type
	{
		get { return "Armor"; }
	}
	public Armor()
	{
	
	}
	public Armor(int id, string name, string desc, int gold, int hp) : base(id, name, desc, gold)
	{
		armorHp = hp;
	}
	
	public override string ToString()
	{
		string s;
		s = base.ToString ();
		s += "<b><color=#4DA4BF>" +  "+HP:" + "</color></b>" + armorHp.ToString() + "\n";
		return s;
	}
    public override void AddStat()
    {
        Player.instance.playerStat.MaxHp += armorHp;
    }
    public override void RemoveStat()
    {
        Player.instance.playerStat.MaxHp -= armorHp;
    }
}
