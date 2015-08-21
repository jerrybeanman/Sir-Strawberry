using UnityEngine;
using System.Collections;
using System;

public class HeadPiece : Armor {
	public override string Type
	{
		get { return "HeadPiece"; }
	}
	public HeadPiece()
	{
	}

	public HeadPiece(int id, string name, string desc, int gold, int hp) : base(id, name, desc, gold, hp)
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
