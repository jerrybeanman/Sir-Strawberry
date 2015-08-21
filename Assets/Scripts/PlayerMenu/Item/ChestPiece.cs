using UnityEngine;
using System.Collections;

public class ChestPiece : Armor {
	public override string Type
	{
		get { return "ChestPiece"; }
	}
	public ChestPiece()
	{
	}
	public ChestPiece(int id, string name, string desc, int gold, int hp) : base(id, name, desc, gold, hp)
	{

	}
    public override void AddStat()
    {
        base.AddStat();
    }
    public override void RemoveStat()
    {
        base.RemoveStat();
    }
}
