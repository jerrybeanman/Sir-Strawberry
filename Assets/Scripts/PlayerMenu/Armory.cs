using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Armory : MonoBehaviour {
	public HeadPiece headPiece;
	public ChestPiece chestPiece;
	public Pair<Weapon, Weapon> weapons;
	public Pair<Comsumable, Comsumable> comsumables;

public class Pair<T, U> {
	public Pair() {}
	
	public Pair(T first, U second) {
		this.First = first;
		this.Second = second;
	}
	
	public T First { get; set; }
	public U Second { get; set; }
};
}
