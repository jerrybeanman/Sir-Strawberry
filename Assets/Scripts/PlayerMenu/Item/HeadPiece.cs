using UnityEngine;
using System.Collections;

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
}
